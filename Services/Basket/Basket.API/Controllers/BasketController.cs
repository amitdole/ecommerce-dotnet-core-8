using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Basket.Application.Responses;
using Basket.Application.Queries;
using Basket.Application.Commands;
using Basket.Core.Entities;
using Basket.Application.Mappers;
using MassTransit;
using EventBus.Messages.Events;
using Asp.Versioning;

namespace Basket.API.Controllers
{
    [ApiVersion("1")]
    public class BasketController : ApiController
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

        [HttpGet]
        [Route("[action]/{userName}", Name = "GetBasketByUserName")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommand command)
        {
            var basket = await _mediator.Send(command);
            return Ok(basket);
        }

        [HttpDelete("[action]/{userName}", Name = "DeleteBasketByUserName")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            var command = new DeleteBasketByUserNameCommand(userName);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            //Get the basket with the username
            var getBasket = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(getBasket);

            if (basket == null)
            {
                return BadRequest();
            }

            //Once basket is received, send basketcheckout event to rabbitmq

            var eventMessage = BasketMapper.Mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);
            _logger.LogInformation($"Basket event published for {basket.UserName}");

            //remove the basket
            var command = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
            await _mediator.Send(command);

            return Accepted();
        }
    }
}
