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
        public CreateSession(string id, string userID)
        {
            ID = id;
            UserID = userID;
        }

        public string ID { get; }
        public string UserID { get; }
    }

    public class DestroySession : Message
    {
        public DestroySession(string id)
        {
            ID = id;
        }

        public string ID { get; }
    }

    public class SessionCreated : Message
    {
        public SessionCreated(string id)
        {
            ID = id;
        }

        public string ID { get; }
    }

    public class SessionDestroyed : Message
    {
        public SessionDestroyed(string id)
        {
            ID = id;
        }

        public string ID { get; }
    }
}
