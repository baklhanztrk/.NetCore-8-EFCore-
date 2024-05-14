namespace EFCore.Arvato.Core.RabbitMq
{
    public interface IRabbitMQServices
    {
      

        Task ListenMessageQueue(string routingKey, string eventData);
    }
}
