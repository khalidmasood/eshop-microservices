using BuildingBlocks.Behaviors;
using FluentValidation;
using System.Runtime.InteropServices;


var myPrgramAssembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container. Registration operations.

builder.Services.AddCarter();


//We add MediatR method to register services from assembly method via => configuation and that tells where to find our command and query handle classes

builder.Services.AddMediatR(config =>
{

    config.RegisterServicesFromAssemblies(myPrgramAssembly);
    config.AddOpenBehavior(typeof(ValidationBehaviors<,>)); // Register any generic one into MediatoR

});

//Add fluent validators
builder.Services.AddValidatorsFromAssembly(myPrgramAssembly);


//Add Marten and configure the database connection using the app settings
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    //opts.AutoCreateSchemaObjects by default it's CreateOrUpdate
}).UseLightweightSessions();//and use the light weigh session


var app = builder.Build();



// Configure HTTP request pipline. Apply Use function or Map function methods in order to configure the HTTP request lifecycle.


app.MapCarter();//it maps the routes defined in the ICarterModule implementation

app.Run();


//And with this we finished configuring and adding the Carter and MediatR libraries




