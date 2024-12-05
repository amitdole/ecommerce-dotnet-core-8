using MediatR;
using Discount.Grpc.Protos;

namespace Discount.Application.Queries
{
    public class GetDiscountQuery : IRequest<CouponModel>
    {
        public string ProductName { get; set; }
        public GetDiscountQuery(string productName)
        {
            ProductName = productName;
        }
    }
}
