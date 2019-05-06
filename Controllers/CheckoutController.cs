using iTextSharp.text;
using iTextSharp.text.pdf;
using Shop.Models;
using Shop.Models.Domain;
using Shop.Models.Domain.Enum;
using Shop.Models.ShoppingCartViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Shop.Filters;
using Shop.Models.Domain.Interface;

namespace Shop.Controllers
{
    [ServiceFilter(typeof(CartSessionFilter))]
    public class CheckoutController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ISellerRepository _sellerRepository;

        public CheckoutController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ICategoryRepository categoryRepository, IUserRepository userRepository, IReceiptRepository receiptRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, ISellerRepository sellerRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _receiptRepository = receiptRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _sellerRepository = sellerRepository;
        }

        public IActionResult Index(string checkoutId, string returnUrl = null)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            switch (checkoutId)
            {
                case "Guest":
                    return RedirectToAction(nameof(CheckoutController.PlacingOrder), "Checkout");
                case "New":
                    return RedirectToAction(nameof(AccountController.Register), "Account", new { ReturnUrl = returnUrl });
                case "LogIn":
                    return RedirectToAction(nameof(AccountController.Login), "Account", new { ReturnUrl = returnUrl });
                default:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateReceipt(int index)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            User user = await RetrieveUser();
            Order order = _orderRepository.GetBy(user.Bills.Last().BillId);
            ICollection<OrderItem> OrderItem = RetrieveOrderItem(order);

            ViewData["Order"] = order;
            ViewData["OrderItem"] = OrderItem;
            ViewData["Index"] = index;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReceipt(int index, CreateOrderViewModel model)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["Index"] = index;
            var user = await RetrieveUser();
            Order order = _orderRepository.GetBy(user.Bills.Last().BillId);
            ViewData["Order"] = order;
            IList<OrderItem> OrderItem = RetrieveOrderItem(order).ToList();
            ViewData["OrderItem"] = OrderItem;

            if (ModelState.IsValid)
            {
                OrderItem orderItem = OrderItem[(int)index];
                orderItem.SenderName = model.Name;
                orderItem.SenderEmail = model.Email;
                orderItem.RecipientName = model.NameReciever;
                if (model.Message != null && model.Message != "")
                    orderItem.Message = model.Message;
                if (model.EmailReciever != null && model.EmailReciever != "")
                    orderItem.RecipientEmail = model.EmailReciever;
                _orderItemRepository.SaveChanges();

                MakeReceipt(orderItem);

                if ((index + 1) == OrderItem.Count)
                    return RedirectToAction(nameof(CheckoutController.Payment), "Checkout", new { Id = order.BillId });
                return RedirectToAction(nameof(CheckoutController.CreateReceipt), "Checkout", new { index = index + 1 });
            }

            return View(model);
        }

        public void GenerateQR(string qrcode)
        {
            var bonPath = @"wwwroot/images/temp/";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save(bonPath + qrcode + ".png", ImageFormat.Png);
        }

        private async Task<User> RetrieveUser()
        {
            var user = _userRepository.GetBy("lekkerlokaal");
            if (_signInManager.IsSignedIn(User))
            {
                var _user = await _userManager.GetUserAsync(User);
                if (_userRepository.GetBy(_user.Email) != null)
                    user = _userRepository.GetBy(_user.Email);
            }
            return user;
        }

        private ICollection<OrderItem> RetrieveOrderItem(Order order)
        {
            ICollection<OrderItem> OrderItem = new HashSet<OrderItem>();

            foreach (OrderItem or in order.Orders)
            {
                OrderItem.Add(_orderItemRepository.GetById(or.OrderItemId));
            }

            return OrderItem;
        }

        public async Task<IActionResult> PlacingOrder(ShoppingCart shoppingCart)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            var user = await RetrieveUser();
            user.PlaceOrder(shoppingCart);
            _userRepository.SaveChanges();
            _receiptRepository.SaveChanges();

            return RedirectToAction(nameof(CheckoutController.CreateReceipt), "Checkout", new { index = 0 });
        }

        public IActionResult Payment(int Id)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();
            ViewData["Index"] = Id;

            return View();
        }

        public IActionResult MakeReceiptsUsable(int Id, ShoppingCart shoppingCart)
        {
            shoppingCart.MakeEmpty();
            Order order = _orderRepository.GetBy(Id);
            ICollection<OrderItem> OrderItem = RetrieveOrderItem(order);
            OrderItem.ToList().ForEach(bl => bl.Validity = Validity.Valid);
            IList<OrderItem> bestellijn = OrderItem.ToList();
            _userRepository.SaveChanges();

            SendMails(order);

            return RedirectToAction(nameof(CheckoutController.Thanks), "Checkout", new { Id });
        }

        public async Task<IActionResult> Thanks(int Id)
        {
            ViewData["AllCategories"] = _categoryRepository.GetAll().ToList();

            Order order = _orderRepository.GetBy(Id);

            if (_signInManager.IsSignedIn(User))
            {
                var applicationUser = await _userManager.GetUserAsync(User);
                var application = _userRepository.GetBy(applicationUser.Email);
                var orderer = _userRepository.GetByOrderId(Id);

                if (application == orderer)
                {
                    ICollection<OrderItem> OrderItem = RetrieveOrderItem(order);
                    IList<OrderItem> orderItemList = OrderItem.ToList();
                    ViewData["OrderItem"] = orderItemList;

                    // step 1: creation of a document-object
                    Document document = new Document();

                    // step 2: we create a writer that listens to the document
                    PdfCopy writer = new PdfCopy(document, new FileStream(@"wwwroot/pdf/merged_" + orderItemList[0].QRCode + ".pdf", FileMode.Create));
                    if (writer != null)
                    {
                        // step 3: we open the document
                        document.Open();

                        foreach (var bestellijn in OrderItem)
                        {
                            // we create a reader for a certain document
                            PdfReader reader = new PdfReader(@"wwwroot/pdf/c_" + bestellijn.QRCode + ".pdf");
                            reader.ConsolidateNamedDestinations();

                            // step 4: we add content
                            for (int i = 1; i <= reader.NumberOfPages; i++)
                            {
                                PdfImportedPage page = writer.GetImportedPage(reader, i);
                                writer.AddPage(page);
                            }

                            //PRAcroForm form = reader.AcroForm;
                            //if (form != null)
                            //{
                            //    writer.CopyAcroForm(reader);
                            //}

                            reader.Close();
                        }

                        // step 5: we close the document and writer
                        writer.Close();
                        document.Close();
                        return View();
                    }
                }
            }
            //anoniem of hackattempt -> geen download knop
            ViewData["OrderItem"] = null;
            return View();
        }

        private void MakeReceipt(OrderItem orderItem)
        {
            var receipt = _receiptRepository.GetByReceiptId(orderItem.Receipt.ReceiptId);
            var seller = _sellerRepository.GetBySellerId(receipt.Seller.SellerId);

            string value = String.Format(orderItem.Price + "vnd");
            string date = orderItem.CreationDate.AddYears(1).ToString("dd/MM/yyyy");
            string valid = String.Format("Co hieu luc: " + date);
            var pdf = new Document(PageSize.A5.Rotate(), 81, 225, 25, 0);
            GenerateQR(orderItem.QRCode);
            var imageURL = @"wwwroot/images/temp/" + orderItem.QRCode + ".png";
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            jpg.ScaleToFit(145f, 145f);
            var logoURL = @"wwwroot/images/logo.png";
            var logoURLSeller = @"wwwroot" + seller.GetLogoPath();
            var kadoURL = @"wwwroot/images/kado.jpg";
            iTextSharp.text.Image kado = iTextSharp.text.Image.GetInstance(kadoURL);
            iTextSharp.text.Image logoLL = iTextSharp.text.Image.GetInstance(logoURL);
            iTextSharp.text.Image logoSeller = iTextSharp.text.Image.GetInstance(logoURLSeller);

            logoLL.SetAbsolutePosition(20, 15);
            logoLL.ScaleToFit(188f, 100f);
            logoSeller.ScaleToFit(188f, 100f);
            logoSeller.SetAbsolutePosition(410, 15);
            jpg.SetAbsolutePosition(225, 0);
            kado.SetAbsolutePosition(65, 161);

            iTextSharp.text.Font arial = FontFactory.GetFont("Arial", 23);
            iTextSharp.text.Font arial18 = FontFactory.GetFont("Arial", 14);
            iTextSharp.text.Font arialSmall = FontFactory.GetFont("Arial", 7);

            Paragraph amount = new Paragraph(value, arial);
            amount.SpacingAfter = 50;
            Paragraph nameSeller = new Paragraph(receipt.Name, arial);
            nameSeller.SpacingAfter = 0;
            Paragraph givenBy = new Paragraph("Tang boi: " + orderItem.SenderName, arial18);
            Paragraph _valid = new Paragraph(valid, arial18);
            amount.Alignment = Element.ALIGN_LEFT;
            nameSeller.Alignment = Element.ALIGN_LEFT;
            givenBy.Alignment = Element.ALIGN_LEFT;
            _valid.Alignment = Element.ALIGN_LEFT;

            Phrase qrCodeString = new Phrase(orderItem.QRCode, arialSmall);

            PdfWriter writer = PdfWriter.GetInstance(pdf, new FileStream(@"wwwroot/pdf/c_" + orderItem.QRCode + ".pdf", FileMode.Create));
            pdf.Open();

            ColumnText.ShowTextAligned(writer.DirectContent,
            Element.ALIGN_MIDDLE, qrCodeString, 195, 4, 0);


            pdf.Add(logoLL);
            pdf.Add(logoSeller);
            pdf.Add(nameSeller);
            pdf.Add(amount);
            pdf.Add(givenBy);
            pdf.Add(_valid);
            pdf.Add(jpg);
            pdf.Add(kado);
            pdf.Close();

            System.IO.File.Delete(imageURL);
        }

        private void SendMails(Order order)
        {
            ICollection<OrderItem> OrderItem = RetrieveOrderItem(order);
            IList<OrderItem> orderItemList = OrderItem.ToList();

            MailMessage message = new MailMessage();
            message.From = new MailAddress("3bros.shop.suport@gmail.com");
            message.To.Add(orderItemList[0].SenderEmail);
            message.Subject = "Đơn đặt hàng của bạn từ 3BrosShop.";
            message.Body = String.Format("Kính " + orderItemList[0].SenderName + ", " + System.Environment.NewLine + System.Environment.NewLine +
                "Cảm ơn bạn đã đặt hàng tại 3BrosShop." + System.Environment.NewLine +
                "Trong tệp đính kèm, bạn sẽ tìm thấy phiếu quà tặng đã mua." + System.Environment.NewLine + System.Environment.NewLine +
                "Trân trọng," + System.Environment.NewLine + "3Bros team");
            var attachment = new Attachment(@"wwwroot/favicon.ico");
            attachment.Dispose();
            for (int i = 0; i < OrderItem.Count; i++)
            {
                attachment = new Attachment(@"wwwroot/pdf/c_" + orderItemList[i].QRCode + ".pdf");
                attachment.Name = "hoadon" + (i + 1) + ".pdf";
                message.Attachments.Add(attachment);

            }
            var SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(message);
            attachment = new Attachment(@"wwwroot/favicon.ico");
            attachment.Dispose();

            for (int i = 0; i < OrderItem.Count; i++)
            {
                if (orderItemList[i].RecipientEmail != null && orderItemList[i].RecipientEmail != "")
                {
                    string _message = "";
                    if (orderItemList[i].Message != null && orderItemList[i].Message != "")
                    {
                        _message = String.Format("Tin nhắn riêng: " + System.Environment.NewLine +
                        orderItemList[i].Message + System.Environment.NewLine + System.Environment.NewLine);
                    }
                    MailMessage message2 = new MailMessage();
                    message2.From = new MailAddress("3bros.shop.suport@gmail.com");
                    message2.To.Add(orderItemList[i].RecipientEmail);
                    message2.Subject = orderItemList[i].SenderName + " gửi cho bạn một phiếu quà tặng từ 3BrosShop.";
                    message2.Body = String.Format(

                        "Kính gửi " + orderItemList[i].RecipientName + ", " + System.Environment.NewLine + System.Environment.NewLine +
                        orderItemList[i].SenderName + " đã tặng phiếu quà tặng cho bạn." + System.Environment.NewLine + System.Environment.NewLine +

                        _message +

                        "Bạn sẽ tìm thấy phiếu quà tặng kèm theo." + System.Environment.NewLine + System.Environment.NewLine +
                        "Trân trọng ," + System.Environment.NewLine + "3Bros team");

                    attachment = new Attachment(@"wwwroot/pdf/c_" + orderItemList[i].QRCode + ".pdf");
                    attachment.Name = "hoadon.pdf";
                    message2.Attachments.Add(attachment);
                    var SmtpServer2 = new SmtpClient("smtp.gmail.com");
                    SmtpServer2.Port = 587;
                    SmtpServer2.Credentials = new System.Net.NetworkCredential("3bros.shop.suport@gmail.com", "1234567893bros");
                    SmtpServer2.EnableSsl = true;
                    SmtpServer2.Send(message2);
                    attachment.Dispose();

                }
            }
        }

    }
}