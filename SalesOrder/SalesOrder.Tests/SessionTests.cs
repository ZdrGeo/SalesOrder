using Akka.TestKit.Xunit2;
using Xunit;
using Akka.Actor;
using Akka.Routing;
using SalesOrder.Messages;
using SalesOrder.Actors;
using System.Threading.Tasks;
using System;

namespace SalesOrder.Tests
{
    public class SessionTests : TestKit
    {
        [Fact]
        public async Task SessionShouldBeDistributed()
        {
            IActorRef sessionRouterActor = ActorOf(Props.Create<SessionCollectionActor>().WithRouter(FromConfig.Instance), "session-router");

            for (int index = 0; index < 10; index++)
            {
                string sessionId = $"{ index }";
                string userId = $"{ index }";

                SessionFound sessionFound = await sessionRouterActor.Ask<SessionFound>(new FindSession(sessionId), TimeSpan.FromSeconds(20));

                if (sessionFound.SessionActor.IsNobody())
                {
                    SessionCreated sessionCreated = await sessionRouterActor.Ask<SessionCreated>(new CreateSession(sessionId, userId), TimeSpan.FromSeconds(20));
                }

                // SessionCreated sessionCreated = await sessionRouterActor.Ask<SessionCreated>(new CreateSession(sessionId, userId), TimeSpan.FromSeconds(20));
            }
        }
    }
}
