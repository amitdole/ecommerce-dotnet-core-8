using MediatR;
using Basket.Application.Responses;

namespace Basket.Application.Queries
{
    public class GetBasketByUserNameQuery : IRequest<ShoppingCartResponse>
    {
        public string UserName { get; set; }

        public GetBasketByUserNameQuery(string userName)
        {
            UserName = userName;
        }
    }
}