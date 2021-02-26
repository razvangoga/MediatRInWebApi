using System;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MediatRInWebApi.Services
{
    public class ProvisionSubscriptionRequest : INotification
    {
        public Guid SubscriptionId { get; set; }
        public bool Crash { get; set; }
    }
    
    public class ProvisionSubscriptionHandler : INotificationHandler<ProvisionSubscriptionRequest>
    {
        private readonly ILogger<ProvisionSubscriptionHandler> _logger;
        private readonly ISignalRService _signalRService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ProvisionSubscriptionHandler(ILogger<ProvisionSubscriptionHandler> logger, ISignalRService signalRService, IServiceScopeFactory serviceScopeFactory)
        {
            this._logger = logger;
            this._signalRService = signalRService;
            this._serviceScopeFactory = serviceScopeFactory;
        }
        
        public async Task Handle(ProvisionSubscriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                int delayInSeconds = 5;
            
                this._logger.LogDebug($"ProvisionSubscriptionHandler start - this will take {delayInSeconds}s");
            
                //do the long running stuff (create db, table etc)
                await Task.Delay(TimeSpan.FromSeconds(delayInSeconds), cancellationToken);

                if (request.Crash)
                    throw new Exception("ka-boom");

                using (IServiceScope scope = this._serviceScopeFactory.CreateScope())
                {
                    IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Publish(new Ok(), cancellationToken);
                }
                
                this._signalRService.Ok(request.SubscriptionId);
            
                this._logger.LogDebug("ProvisionSubscriptionHandler end ok");
            }
            catch (Exception e)
            {
                this._signalRService.Fail(request.SubscriptionId, e.Message);
                this._logger.LogDebug("ProvisionSubscriptionHandler end fail");
            }
        }
    }
}