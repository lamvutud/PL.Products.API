namespace PL.Products.API.Infrastructure.MessageBrokers;

public interface IMessageSender<T>
{
    Task SendAsync(T message, MetaData metaData = null, CancellationToken cancellationToken = default);
}
