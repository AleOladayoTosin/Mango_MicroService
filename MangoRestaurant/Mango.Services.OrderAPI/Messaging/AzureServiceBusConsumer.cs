using Azure.Messaging.ServiceBus;
using Mango.Services.OrderAPI.Messages;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Mango.MessageBus;

namespace Mango.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionName;
        private readonly string checkoutMessageTopic;
        private readonly string orderPaymentProcessTopic;
        private readonly IConfiguration _configuration;
        private readonly OrderRepository _orderRepository;
        private readonly IMessageBus _messageBus;

        private ServiceBusProcessor checkoutProcessor;
        public AzureServiceBusConsumer(OrderRepository orderRepository, 
            IConfiguration configuration, IMessageBus messageBus)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;
            _messageBus = messageBus;   

            serviceBusConnectionString = configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionName = configuration.GetValue<string>("CheckoutMessageSubscriptionName");
            checkoutMessageTopic = configuration.GetValue<string>("CheckoutMessageTopic");
            orderPaymentProcessTopic = configuration.GetValue<string>("OrderPaymentProcessTopic");

            var client = new ServiceBusClient(serviceBusConnectionString);

            checkoutProcessor = client.CreateProcessor(checkoutMessageTopic, subscriptionName);
        }

        public async Task Start()
        {
            checkoutProcessor.ProcessMessageAsync += OnCheckOutMessageRecieved;
            checkoutProcessor.ProcessErrorAsync += ErrorHanler;
            await checkoutProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await checkoutProcessor.StopProcessingAsync();
            await checkoutProcessor.DisposeAsync();
        }
        Task ErrorHanler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        private async Task OnCheckOutMessageRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;

            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = new()
            {
                UserId = checkoutHeaderDto.UserId,
                FirstName = checkoutHeaderDto.FirstName,
                LastName = checkoutHeaderDto.LastName,
                orderDetails = new List<OrderDetails>(),
                CardNumber = checkoutHeaderDto.CardNumber,
                CouponCode = checkoutHeaderDto.CouponCode,
                CVV = checkoutHeaderDto.CVV,
                DiscountTotal = checkoutHeaderDto.DiscountTotal,
                Email = checkoutHeaderDto.Email,
                ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
                DateCreated = DateTime.Now,
                OrderTotal = checkoutHeaderDto.OrderTotal,  
                Phone = checkoutHeaderDto.Phone,    
                PaymentStatus = false,
                PickUpDateTime = checkoutHeaderDto.PickUpDateTime,
            };
            foreach (var item in checkoutHeaderDto.cartDetails)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    Count = item.Count,
                };
                orderHeader.CartTotalItem += item.Count;
                orderHeader.orderDetails.Add(orderDetails); 
            }

            await _orderRepository.AddOrder(orderHeader);

            PaymentRequestMessage paymentRequestMessage = new()
            {
                Name = orderHeader.FirstName + " " + orderHeader.LastName,
                CardNumber = orderHeader.CardNumber,
                CVV = orderHeader.CVV,
                ExpiryMonthYear = orderHeader.ExpiryMonthYear,
                OrderId = orderHeader.OrderHeaderId,
                OrderTotal = orderHeader.OrderTotal
            };

            try
            {
                await _messageBus.PublishMessage(paymentRequestMessage, orderPaymentProcessTopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {}
        }
    }
}
