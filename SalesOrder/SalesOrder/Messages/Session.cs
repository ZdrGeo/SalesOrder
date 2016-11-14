using System;
using Akka.Actor;
using Akka.Routing;

namespace SalesOrder.Messages
{
    public abstract class ConsistentHashableMessage : Message, IConsistentHashable
    {
        public ConsistentHashableMessage(string sessionId)
        {
            SessionId = sessionId;
        }

        public string SessionId { get; }

        public object ConsistentHashKey => SessionId;
    }

    public abstract class SessionMessage : ConsistentHashableMessage
    {
        public SessionMessage(string sessionId, IActorRef sessionActor) : base (sessionId)
        {
            SessionActor = sessionActor;
        }

        public IActorRef SessionActor { get; }
    }

    public class CreateSession : ConsistentHashableMessage
    {
        public CreateSession(string sessionId, string userId) : base (sessionId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }

    public class SessionCreated : SessionMessage
    {
        public SessionCreated(string sessionId, IActorRef sessionActor) : base (sessionId, sessionActor) { }
    }

    public class DestroySession : ConsistentHashableMessage
    {
        public DestroySession() : base (string.Empty) { }
        public DestroySession(string sessionId) : base (sessionId) { }
    }

    public class SessionDestroyed : ConsistentHashableMessage
    {
        public SessionDestroyed(string sessionId) : base (sessionId) { }
    }

    public class FindSession : ConsistentHashableMessage
    {
        public FindSession(string sessionId) : base (sessionId) { }
    }

    public class SessionFound : SessionMessage
    {
        public SessionFound(string sessionId, IActorRef sessionActor) : base (sessionId, sessionActor) { }
    }
}
