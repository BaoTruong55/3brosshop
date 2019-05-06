using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Shop.Services;

namespace Shop.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link, string name)
        {
            return emailSender.SendEmailAsync(email, "Xác nhận địa chỉ email",
                $"<p>Chào {name}!</p>" +
                $"<p>Cảm ơn bạn đã đăng ký 3Bros Shop.</p>" +
                $"<p>Vui lòng <a href='{HtmlEncoder.Default.Encode(link)}'>nhấp vào đây</a> ođể xác nhận địa chỉ email của bạn để tài khoản của bạn có thể được tạo.</p>" +
                $"<p>Nếu điều này không thành công, hãy nhấn vào liên kết này: <a href='{HtmlEncoder.Default.Encode(link)}'>{HtmlEncoder.Default.Encode(link)}</a></p>" + 
                $"<p> Nếu bạn không phải là người muốn tạo tài khoản tại 3Bros Shop , bạn có thể bỏ qua email này. </p>" +
                $"<p>Trân trọng, </p>" +
                $"<p>3Bros team </p>");
        }
    }
}
