using MediatR;
using Discount.Grpc.Protos;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.Application.Handlers
{
    public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<GetDiscountQueryHandler> _logger;

        public GetDiscountQueryHandler(IDiscountRepository discountRepository, ILogger<GetDiscountQueryHandler> logger)
        {
            _discountRepository = discountRepository;
            _logger = logger;
        }
        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }

            _logger.LogInformation($"Coupon for produc : {request.ProductName} for Amount : {coupon.Amount} applied");

            return new CouponModel
            {
                Id = coupon.Id,
                Amount = coupon.Amount,
                Description = coupon.Description,
                ProductName = coupon.ProductName
            };
        }
    }
}