using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MediatRInWebApi.Services
{
    public class CreateSubscriptionRequest : IRequest<CreateSubscriptionResponse>
    {
        public Guid SubscriptionId { get; set; }
        public bool Crash { get; set; }
    }

    public class CreateSubscriptionResponse
    {
        public Guid SubscriptionId { get; set; }
        public string Status { get; set; }
    }

    public class CreateSubscriptionHandler : IRequestHandler<CreateSubscriptionRequest, CreateSubscriptionResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateSubscriptionHandler> _logger;

        public CreateSubscriptionHandler(IMediator mediator, ILogger<CreateSubscriptionHandler> logger)
        {
            this._mediator = mediator;
            this._logger = logger;
        }

        public Task<CreateSubscriptionResponse> Handle(CreateSubscriptionRequest request, CancellationToken cancellationToken)
        {
            this._logger.LogDebug("CreateSubscriptionHandler start");

            Task.Run(() => this._mediator.Publish(new ProvisionSubscriptionRequest()
            {
                SubscriptionId = request.SubscriptionId,
                Crash = request.Crash
            }, cancellationToken), cancellationToken);

            this._logger.LogDebug("CreateSubscriptionHandler end");

            return Task.FromResult(new CreateSubscriptionResponse()
            {
                SubscriptionId = request.SubscriptionId,
                Status = "subscription saved"
            });
        }
    }
}