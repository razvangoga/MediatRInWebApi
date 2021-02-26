using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MediatRInWebApi.Services
{
    public class Ok : INotification
    {
        
    }
    
    public class SignalRNotificationService : INotificationHandler<Ok>
    {
        private readonly ILogger<SignalRNotificationService> _logger;

        public SignalRNotificationService(ILogger<SignalRNotificationService> logger)
        {
            this._logger = logger;
        }
        
        public Task Handle(Ok notification, CancellationToken cancellationToken)
        {
            this._logger.LogDebug($"SignalRNotificationService {nameof(Ok)}");
            return Task.CompletedTask;
        }
    }
}