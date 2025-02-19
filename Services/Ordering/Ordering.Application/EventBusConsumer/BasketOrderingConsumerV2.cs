using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;

namespace Ordering.Application.EventBusConsumer
{
    public class BasketOrderingConsumerV2 : IConsumer<BasketCheckoutEventV2>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketOrderingConsumerV2> _logger;

        public BasketOrderingConsumerV2(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumerV2> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
        {
            _logger.LogInformation($"Consuming Basket Checkout V2 event: {context.Message.CorrelationId} for {context.Message.UserName}");

            var command = _mapper.Map<CheckoutOrderCommandV2>(context.Message);
            var result = await _mediator.Send(command);

            _logger.LogInformation("Basket Checkout V2 event consumed");
        }
    }
}