using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct
{


    //Create command data object
    //So when MediatR see this request comes from the API Request,  it will trigger CreateProductCommandHandler
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;

    //Create result data object
    public record CreateProductResult(Guid Id);

    //In order to trigger this command handler, this needs to implemet the interface IRequestHandler<CreateProductCommand, CreateProductResult>
    internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {

            //Business logic to create a product

            //1) create product entity from command object
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
            };

            //2) save to database

            session.Store(product);

            await session.SaveChangesAsync(cancellationToken);

            //3) return CreateProductResult result

            return new CreateProductResult(product.Id);
        }
    }
}
