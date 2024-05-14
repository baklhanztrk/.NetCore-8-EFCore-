using EFCore.Arvato.Context;
using EFCore.Arvato.Core.Auth;
using EFCore.Arvato.Core.Orders;
using EFCore.Arvato.Core.RabbitMq;
using EFCore.Arvato.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;
using System.Threading;

namespace EFCore.Arvato.Services.RabbitMq
{
    public class RabbitMqServices : IRabbitMQServices
    {
        
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        public RabbitMqServices(IServiceScopeFactory serviceScopeFactory,IConfiguration configuration)
        {

           
            _scopeFactory = serviceScopeFactory;
            _configuration=configuration;
        }
             


        public async Task ListenMessageQueue(string routingKey, string eventData)
        {

            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMq:HostName"],
                UserName = _configuration["RabbitMq:UserName"],
                Password = _configuration["RabbitMq:Password"]
                //port=15672
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();
            channel.QueueDeclare("ActivationQueue", true, false, false, null);

            var cosumer = new EventingBasicConsumer(channel);
            
            if (channel.MessageCount("ActivationQueue") > 0)
            {
                  cosumer.Received += async  (model, deliveryEventArgs) =>
                {
                    var body = Encoding.UTF8.GetString(deliveryEventArgs.Body.ToArray());
                     await ParseInvertoryOrderMessage(body, deliveryEventArgs);

                };

                channel.BasicConsume("ActivationQueue", true, cosumer);
                await Task.CompletedTask;

            }          
            

        }

        private async Task ParseInvertoryOrderMessage(string message,BasicDeliverEventArgs deliveryEventArgs)
        {
            using var scrope = _scopeFactory.CreateScope();
            var orderDb = scrope.ServiceProvider.GetRequiredService<MyDbContext>();
            var data = JObject.Parse(message);


            var orderId = data["orderId"].Value<int>();
            var recordOrder = await orderDb.Orders.FirstOrDefaultAsync(k => k.OrderId == orderId);

            if (recordOrder is null) {

                await orderDb.Orders.AddAsync(new Order
                {
                    AccountId = data["accountId"].Value<int>(),
                    OrderId = data["orderId"].Value<int>(),
                    OrderNumber = data["orderNumber"].Value<string>(),
                    OrderType = "b2c",
                    Status = "Received",
                    SalesChannel = data["salesChannel"].Value<string>(),
                    UserId = data["userId"].Value<int>(),
                    OrderDate = data["orderDate"].Value<DateTime>(),
                    Carrier = data["carrier"].Value<string>(),
                    City = data["city"].Value<string>(),
                    District = data["district"].Value<string>(),
                    CreatedAt = data["userId"].Value<int>()
                });

                await orderDb.SaveChangesAsync();
            }
                        

              await Task.Delay(new Random().Next(1, 3) * 1000);
               
            

        }
    }
}
