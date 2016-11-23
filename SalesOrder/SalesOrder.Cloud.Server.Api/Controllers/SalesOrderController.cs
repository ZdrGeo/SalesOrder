using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using Akka.Actor;

using SalesOrder.Messages;

namespace SalesOrder.Cloud.Server.Api.Controllers
{
    public class SalesOrderController : ApiController
    {
        public Task<string> Get(int id)
        {
            return Task.FromResult(string.Empty);
        }

        public async Task Post([FromBody] string value)
        {
            for (int index = 0; index < 10; index++)
            {
                string sessionId = $"{ index }";
                string userId = $"{ index }";

                SessionFound sessionFound = await SalesOrderActorSystem.SessionRouterActor.Ask<SessionFound>(new FindSession(sessionId), TimeSpan.FromSeconds(20));

                if (sessionFound.SessionActor.IsNobody())
                {
                    SessionCreated sessionCreated = await SalesOrderActorSystem.SessionRouterActor.Ask<SessionCreated>(new CreateSession(sessionId, userId), TimeSpan.FromSeconds(20));
                }

                // SessionCreated sessionCreated = await SalesOrderActorSystem.SessionRouterActor.Ask<SessionCreated>(new CreateSession(sessionId, userId), TimeSpan.FromSeconds(20));
            }
        }

        public Task Put(int id, [FromBody] string value)
        {
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            return Task.CompletedTask;
        }
    }
}
