namespace SalesOrder.Messages
{
    public abstract class Message { }

    public class DeliverAtLeastOnce<T>
    {
        public DeliverAtLeastOnce(long deliveryId, T message)
        {
            DeliveryId = deliveryId;
            Message = message;
        }

        public long DeliveryId { get; }

        public T Message { get; }
    }

    public class AtLeastOnceDelivered
    {
        public AtLeastOnceDelivered(long deliveryId)
        {
            DeliveryId = deliveryId;
        }

        public long DeliveryId { get; }
    }
}
