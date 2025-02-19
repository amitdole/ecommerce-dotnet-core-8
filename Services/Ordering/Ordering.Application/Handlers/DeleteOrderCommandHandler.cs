using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderId = await _orderRepository.GetByIdAsync(request.Id);

            if (orderId == null)
            {
                _logger.LogError($"Order {request.Id} is not found.");
                throw new OrderNotFoundException(nameof(Order), request.Id);
            }

            await _orderRepository.DeleteAsync(orderId);
            _logger.LogInformation($"Order {request.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
