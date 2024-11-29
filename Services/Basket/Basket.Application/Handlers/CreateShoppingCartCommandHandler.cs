using MediatR;
using Basket.Application.Responses;
using Basket.Application.Commands;
using Basket.Core.Repositories;
using Basket.Application.Mappers;

namespace Basket.Application.Handlers
{
    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepository;

        public CreateShoppingCartCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketRepository.UpdateBasket(new Core.Entities.ShoppingCart
            {
                UserName = request.UserName,
                Items = request.Items
            });

            var shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(shoppingCart);

            return shoppingCartResponse;
        }
    }
}
