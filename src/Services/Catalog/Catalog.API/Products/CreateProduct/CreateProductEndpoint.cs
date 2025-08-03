namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{

    //<Summary>
    //This class implements the AddRoutes that exposes the endpoints from our minimal API
    //So the POST endpoint for create product will be implemented here

    public void AddRoutes(IEndpointRouteBuilder app)
    {

        //Now in here we need a mechanism to Map our request object 
        //The idea is we will use the Mediator library and trigger the Mediator Command Handler
        //So that means we should pass the values from the request object "CreateProductRequest" to the handled command object "CreateProductCommand"
        //In order to pass values from the Request to Command object we need a Mapper
        //So Add the Mapping library Mapster to the BuildingBlocks project that will convert the Request to Command / Query object and wise versa
        //Mapster says writing mapping methods is a machine job, do not waste your time, let Mapster do it.



        app.MapPost("/products",
            async (CreateProductRequest request, ISender sender) =>
            {

                //so 1st we convert to the request to command object or request to command object mapping
                var command = request.Adapt<CreateProductCommand>();

                //Why we needed the command object, because sender (which is the mediator) requires command object to map to the handler

                //we will trigger our mediator and gets the result
                var result = await sender.Send(command);

                //and after getting the response, we can again convert the response object from "CreateProductResult" to the "CreateProductResponse" using the Mapster's adapt method.
                var response = result.Adapt<CreateProductResponse>();


                //returns the 202 Created response
                return Results.Created($"/products/{response.Id}", response);

            })
            //USing extension methods from Carter
        .WithName("CreateProduct")// Name of the API endpoint 
        .Produces<CreateProductResponse>(StatusCodes.Status201Created) // Status should be 201 Created
        .ProducesProblem(StatusCodes.Status400BadRequest) //In case of problem
        .WithSummary("Create Product")//Define doc summary  
        .WithDescription("Create Product");//Define doc description
    }
}