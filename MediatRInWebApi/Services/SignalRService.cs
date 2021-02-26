using System;
using Microsoft.Extensions.Logging;

namespace MediatRInWebApi.Services
{
    public interface ISignalRService
    {
        void Ok(Guid id);
        void Fail(Guid id, string error);
    }
    
    public class SignalRService : ISignalRService
    {
        private readonly ILogger<SignalRService> _logger;

        public SignalRService(ILogger<SignalRService> logger)
        {
            this._logger = logger;
        }
        
        public void Ok(Guid id)
        {
            this._logger.LogDebug($"Sent {nameof(Ok)} {id}");    
        }
        
        public void Fail(Guid id, string error)
        {
            this._logger.LogDebug($"Sent {nameof(this.Fail)} {id} - {error}");    
        }        
    }
}