using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Akka.Actor;
using Akka.Event;
using Akka.DI.Core;

using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;

namespace SalesOrder.Actors
{
    public class SessionActor : ReceiveActor
    {
        private readonly ILoggingAdapter logger = Context.GetLogger();
        private ICancelable cancelable;
        private string sessionId;
        private string userId;
        // private IActorRef SalesOrderCollectionActor;

        public SessionActor()
        {
            // SalesOrderCollectionActor = Context.ActorSelection("/sales-order-collection").ResolveOne(TimeSpan.FromSeconds(10)).Result;

            Receive<CreateSession>(message => CreateSession(message));
            Receive<DestroySession>(message => DestroySession(message));
        }

        private void CreateSession(CreateSession createSession)
        {
            // logger.Info("Create session (Session Id: {0}, User Id: {1})", createSession.SessionId, createSession.UserId);
            Console.WriteLine("Create session (Session Id: {0}, User Id: {1})", createSession.SessionId, createSession.UserId);

            sessionId = createSession.SessionId;
            userId = createSession.UserId;

            SessionCreated sessionCreated = new SessionCreated(createSession.SessionId, Self);

            Sender.Tell(sessionCreated);
        }

        private void DestroySession(DestroySession destroySession)
        {
            // logger.Info("Destroy session (Session Id: {0})", sessionId);
            Console.WriteLine("Destroy session (Session Id: {0})", sessionId);

            // ReleaseLock releaseLock = new ReleaseLock(sessionId, Self, ActorRefs.Nobody);

            // SalesOrderCollectionActor.Tell(releaseLock);

            if (!Sender.IsNobody())
            {
                SessionDestroyed sessionDestroyed = new SessionDestroyed(sessionId);

                Sender.Tell(sessionDestroyed);
            }

            Context.Stop(Self);
        }

        protected override void PreStart()
        {
            DestroySession destroySession = new DestroySession();

            cancelable = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20), Self, destroySession, ActorRefs.Nobody);

            base.PreStart();
        }

        protected override void PostStop()
        {
            cancelable?.Cancel(false);

            base.PostStop();
        }
    }
}
