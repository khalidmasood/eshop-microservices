using BuildingBlocks.Exceptions.Handler;
using Catalog.API.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var myPrgramAssembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container. Registration operations.

builder.Services.AddCarter();


//We add MediatR method to register services from assembly method via => configuation and that tells where to find our command and query handle classes

builder.Services.AddMediatR(config =>
{
    //Steps in MediatR pipeline
    config.RegisterServicesFromAssemblies(myPrgramAssembly);
    config.AddOpenBehavior(typeof(ValidationBehaviors<,>)); // Register any generic one into MediatoR for Validation
    config.AddOpenBehavior(typeof(LoggingBehavior<,>)); // Register any generic one into MediatoR for Logging, ensures that the logging is applied to every MediatR request
});

//Add fluent validators
builder.Services.AddValidatorsFromAssembly(myPrgramAssembly);


//Add Marten and configure the database connection using the app settings
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    //opts.AutoCreateSchemaObjects by default it's CreateOrUpdate
}).UseLightweightSessions();//and use the light weigh session

//if (builder.Environment.IsDevelopment()) {
//    builder.Services.InitializeMartenWith<CatalogInitialData>();
//}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();


// Configure HTTP request pipline. Apply Use function or Map function methods in order to configure the HTTP request lifecycle.

app.MapCarter();//it maps the routes defined in the ICarterModule implementation

//Here we configure our exception handler and the empty option handler means that we are relying on our custom exception handler
app.UseExceptionHandler(option => { });

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });


app.Run();

//And with this we finished configuring and adding the Carter and MediatR libraries




