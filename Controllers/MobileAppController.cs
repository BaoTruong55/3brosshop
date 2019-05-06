﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Models;
using Shop.Models.Domain;
using Shop.Models.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Models.Domain.Interface;

namespace Shop.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MobileAppController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISellerRepository _sellerRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IReceiptRepository _receiptRepository;

        public MobileAppController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ISellerRepository sellerRepository,
            IOrderItemRepository orderItemRepository,
            IReceiptRepository receiptRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _sellerRepository = sellerRepository;
            _orderItemRepository = orderItemRepository;
            _receiptRepository = receiptRepository;
        }

        //GET for registering a seller
        [HttpGet("{id}/{ww}", Name = "ReportSeller")]
        public async Task<Object> ReportSeller(string id, string ww)
        {
            if (_sellerRepository.GetByEmail(id) != null)
            {
                var seller = _sellerRepository.GetByEmail(id);
                var user = await _userManager.FindByEmailAsync(seller.EmailAddress);

                if (await _userManager.CheckPasswordAsync(user, ww))
                {
                    return new
                    {
                        SellerId = seller.SellerId,
                        Description = seller.Description,
                        EmailAddress = seller.EmailAddress,
                        Name = seller.Name
                    };
                }
            }
            return null;
        }

        //GET for order
        [HttpGet("{id}", Name = "PickupOrder")]
        public Object PickupOrder(string id)
        {
            if (_orderItemRepository.GetBy(id) != null)
            {
                var orderItem = _orderItemRepository.GetBy(id);
                if (orderItem != null)
                {
                    var receipt = _receiptRepository.GetByReceiptId(orderItem.Receipt.ReceiptId);
                    var seller = _sellerRepository.GetBySellerId(receipt.Seller.SellerId);
                    return new
                    {
                        OrderId = orderItem.OrderItemId,
                        Name = receipt.Name,
                        orderItem.Price,
                        orderItem.CreationDate,
                        SellerId = seller.SellerId,
                        Email = seller.EmailAddress,
                        orderItem.Validity,
                        ReceiptId = receipt.ReceiptId,
                        image = receipt.GetThumbPath()
                    };
                }
            }
            return null;
        }

        //PUT for receipt
        [HttpPut("{id}", Name = "UpdateOrderItem")]
        public void UpdateOrderItem(int id, [FromBody] OrderModel model)
        {
            if (_orderItemRepository.GetById(id) != null)
            {
                var orderItem = _orderItemRepository.GetById(id);
                if (orderItem != null)
                {
                    orderItem.Seller = _sellerRepository.GetBySellerId(model.SellerId);
                    orderItem.Validity = model.determineValidity();
                    if (orderItem.Validity == Validity.Used)
                        orderItem.ExpirationDate = DateTime.Today;
                    _orderItemRepository.SaveChanges();
                }
            }
        }

        //Model for the PUT
        public class OrderModel
        {
            public int SellerId { get; set; }
            public int Validity { get; set; }

            public Validity determineValidity()
            {
                switch (Validity)
                {
                    case 0:
                        return Models.Domain.Enum.Validity.Valid;
                    case 1:
                        return Models.Domain.Enum.Validity.Invalid;
                    case 2:
                        return Models.Domain.Enum.Validity.Expired;
                    case 3:
                        return Models.Domain.Enum.Validity.Used;
                }

                return Models.Domain.Enum.Validity.Invalid;
            }
        }

    }
}
