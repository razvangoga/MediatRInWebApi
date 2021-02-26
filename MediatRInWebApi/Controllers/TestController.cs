using System;
using System.Threading.Tasks;
using MediatR;
using MediatRInWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MediatRInWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TestController> _logger;

        public TestController(IMediator mediator, ILogger<TestController> logger)
        {
            this._mediator = mediator;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await this.DoStuff(false);
            return this.Ok();
        }
        
        [HttpGet]
        [Route("fail")]
        public async Task<IActionResult> Fail()
        {
            await this.DoStuff(true);
            return this.Ok();
        }

        private async Task DoStuff(bool crash)
        {
            Guid subscriptionId = Guid.NewGuid();
            
            this._logger.LogDebug($"Controller start - {subscriptionId}");

            CreateSubscriptionResponse response = await this._mediator.Send(new CreateSubscriptionRequest()
            {
                SubscriptionId = subscriptionId,
                Crash = crash
            });
            
            this._logger.LogDebug($"Controller end - {response.Status}");
        }
    }
}