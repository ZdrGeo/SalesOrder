using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Akka.Configuration;
using Akka.Actor;
using System.Fabric.Description;

namespace SalesOrder.ServiceFabric.Server
{
    internal class AkkaClusterCommunicationListener : ICommunicationListener
    {
        private const string endpointName = "ServiceEndpoint";
        private readonly Config akkaConfig;
        private readonly ICodePackageActivationContext codePackageActivationContext;
        private readonly NodeContext nodeContext = FabricRuntime.GetNodeContext();
        private ActorSystem actorSystem;

        internal AkkaClusterCommunicationListener(StatelessServiceContext context)
        {
            if (context == null) { throw new ArgumentNullException("context"); }

            codePackageActivationContext = context.CodePackageActivationContext;
            // akkaConfig = AkkaConfiguration.GetAkkaConfiguration(Constants.AkkaConfigSection);
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            EndpointResourceDescription endpoint = codePackageActivationContext.GetEndpoint(endpointName);

            var clusterConfig = nodeContext.ConfigureSeedNodes(akkaConfig, endpoint);
            var heliosConfig = nodeContext.ConfigureHelios(endpoint);

            var config = heliosConfig.WithFallback(clusterConfig).WithFallback(akkaConfig);

            actorSystem = ActorSystem.Create(akkaConfig.GetString(Constants.Akka.SystemName));

            return Task.FromResult(nodeContext.GetSelfAddress(akkaConfig, endpoint));
        }

        public async Task CloseAsync(CancellationToken cancellationToken)
        {
            await actorSystem?.Terminate();

            actorSystem?.Dispose();
        }

        public void Abort()
        {
            actorSystem?.Dispose();
        }
    }

    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Server : StatelessService
    {
        public Server(StatelessServiceContext context) : base (context) { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[] { AkkaClusterCommunicationListener.Create() };
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
