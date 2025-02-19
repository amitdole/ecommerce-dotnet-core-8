using Asp.Versioning;
using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint, ILogger<BasketController> logger)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckout)
        {
            //Get the basket with the username
            var getBasket = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(getBasket);

            if (basket == null)
            {
                return BadRequest();
            }

            //Once basket is received, send basketcheckout event to rabbitmq

            var eventMessage = BasketMapper.Mapper.Map<BasketCheckoutEventV2>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);
            _logger.LogInformation($"Basket event published for {basket.UserName} with V2 endpoint");

            //remove the basket
            var command = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
            await _mediator.Send(command);

            return Accepted();
        }
    }
}