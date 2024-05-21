using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeKeys:SecretKey"];
            var Basket = await _basketRepository.GetBasketAsync(basketId);
            if (Basket == null) return null;
            var shippingPrice = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }

            if (Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != productItem.Price)
                    {
                        item.Price = productItem.Price;
                    }
                }
            }

            var SubTotal = Basket.Items.Sum(item => item.Price * item.Quantity);

            // Create Payment Intent
            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId)) // Create
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)SubTotal + (long)shippingPrice,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await Service.CreateAsync(options);
                // Add PaymentIntentId and ClientSecret created by Stripe to Basket that would return to the Frontend
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)SubTotal + (long)shippingPrice
                };
                paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, options);
                // Add PaymentIntentId and ClientSecret updated by Stripe to Basket that would return to the Frontend
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            await _basketRepository.UpdateOrCreateBasketAsync(Basket);   // Update Basket
            return Basket;
        }
    }
}