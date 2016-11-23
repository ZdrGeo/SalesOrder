using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrder.Actors
{
    public class TraceLogger : ReceiveActor
    {
        public TraceLogger()
        {
            Receive<InitializeLogger>(message =>
            {
                Trace.WriteLine("Initialize trace logger.");

                Sender.Tell(new LoggerInitialized());
            });

            Receive<LogEvent>(@event =>
            {
                Trace.WriteLine(@event);
            });
        }
    }
}
