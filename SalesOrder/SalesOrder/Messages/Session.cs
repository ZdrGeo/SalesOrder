using Akka.Actor;

namespace SalesOrder.Messages
{
    public abstract class SessionMessage : Message
    {
        public SessionMessage(IActorRef sessionActor)
        {
            SessionActor = sessionActor;
        }

        public IActorRef SessionActor { get; private set; }
    }

    public class CreateSession : Message
    {
        public CreateSession(string id, string userId)
        {
            Id = id;
            UserId = userId;
        }

        public string Id { get; }
        public string UserId { get; }
    }

    public class SessionCreated : Message
    {
        public SessionCreated(string id, IActorRef sessionActor)
        {
            Id = id;
            SessionActor = sessionActor;
        }

        public string Id { get; }
        public IActorRef SessionActor { get; }
    }

    public class DestroySession : Message
    {
        public DestroySession(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    public class FindSession
    {
        public FindSession(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    public class SessionFound
    {
        public SessionFound(string id, IActorRef sessionActor)
        {
            Id = id;
            SessionActor = sessionActor;
        }

        public string Id { get; }
        public IActorRef SessionActor { get; }
    }
}
