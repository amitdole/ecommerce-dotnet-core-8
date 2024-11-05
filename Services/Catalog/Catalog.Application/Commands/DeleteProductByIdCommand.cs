using MediatR;

namespace Catalog.Application.Commands
{
    public class DeleteProductByIdCommand : IRequest<bool>
    {
        public DeleteProductByIdCommand(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
    }
}
