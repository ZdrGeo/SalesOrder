using Akka.Actor;

namespace SalesOrder.Messages
{
    public class CreateClient : ProcessMessage
    {
        public CreateClient(IActorRef processActor, string name) : base (processActor)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class ClientCreated : ProcessMessage
    {
        public ClientCreated(IActorRef processActor, string id) : base (processActor)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
