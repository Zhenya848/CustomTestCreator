namespace PetProject.Core.Application.Messaging
{
    public interface IMessageQueue<TMessage>
    {
        public Task WriteAsync(TMessage message, CancellationToken cancellationToken = default);

        public Task<TMessage> ReadAsync(CancellationToken cancellationToken = default);
    }
}
