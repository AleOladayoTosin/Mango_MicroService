using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Mango.MessageBus;
using PaymentProcessor;
using Mango.Service.PaymentAPI.Messages;

namespace Mango.Service.PaymentAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionName;
        private readonly string orderPaymentTopic;
        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;
        private readonly IProcessPayment _payment;

        private ServiceBusProcessor orderProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration, 
            IMessageBus messageBus, IProcessPayment payment)
        {
            _configuration = configuration;
            _messageBus = messageBus;   
            _payment = payment;

            serviceBusConnectionString = configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionName = configuration.GetValue<string>("oladayoPayment");
            orderPaymentTopic = configuration.GetValue<string>("orderpaymentprocesstopic");

            var client = new ServiceBusClient(serviceBusConnectionString);

            orderProcessor = client.CreateProcessor(orderPaymentTopic, subscriptionName);
        }

        public async Task Start()
        {
            orderProcessor.ProcessMessageAsync += OnProcessPaymentRecieved;
            orderProcessor.ProcessErrorAsync += ErrorHanler;
            await orderProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await orderProcessor.StopProcessingAsync();
            await orderProcessor.DisposeAsync();
        }
        Task ErrorHanler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        private async Task OnProcessPaymentRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;

            var body = Encoding.UTF8.GetString(message.Body);

            PaymentRequestMessage paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

            var result = await _payment.PaymentProcessor();

            UpdatePaymentResultMessage updatePaymentResultMessage = new()
            {
                Status = result,
                OrderId = paymentRequestMessage.OrderId,
            };

            try
            {
                await _messageBus.PublishMessage(updatePaymentResultMessage, "topic");
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
