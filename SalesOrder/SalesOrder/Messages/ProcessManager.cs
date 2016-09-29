using Akka.Actor;

namespace SalesOrder.Messages
{
    public abstract class ProcessMessage : Message
    {
        public ProcessMessage(IActorRef processActor)
        {
            ProcessActor = processActor;
        }

        public IActorRef ProcessActor { get; }
    }
}
