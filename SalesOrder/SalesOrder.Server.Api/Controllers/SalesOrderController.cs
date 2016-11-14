using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using Akka.Actor;

using SalesOrder.Messages;

namespace SalesOrder.Server.Api.Controllers
{
    public class SalesOrderController : ApiController
    {
        public async Task<IEnumerable<string>> Get()
        {
            for (int index = 0; index < 10; index++)
            {
                string sessionId = $"{ index }";
                string userId = $"{ index }";

                SessionFound sessionFound = await SalesOrderActorSystem.SessionRouterActor.Ask<SessionFound>(new FindSession(sessionId), TimeSpan.FromSeconds(2));

                if (sessionFound.SessionActor.IsNobody())
                {
                    SessionCreated sessionCreated = await SalesOrderActorSystem.SessionRouterActor.Ask<SessionCreated>(new CreateSession(sessionId, userId), TimeSpan.FromSeconds(2));
                }

                // SessionCreated sessionCreated = await SalesOrderActorSystem.SessionRouterActor.Ask<SessionCreated>(new CreateSession(sessionId, userId), TimeSpan.FromSeconds(2));
            }

            return new string[] { "value1", "value2" };
        }

        public string Get(int id)
        {
            return "value";
        }

        public void Post([FromBody] string value)
        {
        }

        public void Put(int id, [FromBody] string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}
