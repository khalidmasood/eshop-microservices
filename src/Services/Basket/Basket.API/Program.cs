
var myPrgramAssembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);


// Before Building the Application:
// Add services to the DI container. Registration operations.

// Add services to the DI container. Registration operations.

//Add Carter
builder.Services.AddCarter();

//Add MediatR method to register services from assembly method via => configuation and that tells where to find our command and query handle classes
builder.Services.AddMediatR(config =>
{
    //Steps in MediatR pipeline
    config.RegisterServicesFromAssemblies(myPrgramAssembly);
    config.AddOpenBehavior(typeof(ValidationBehaviors<,>)); // Register any generic one into MediatoR for Validation
    config.AddOpenBehavior(typeof(LoggingBehavior<,>)); // Register any generic one into MediatoR for Logging, ensures that the logging is applied to every MediatR request
});

//Add Marten and configure the database connection using the app settings to use PostgreSQL
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName); // so we configured the identity field for ShoppingCart to username in the function expression
}).UseLightweightSessions();//and use the Marten's lightweigh session mode to optimize performance




var app = builder.Build();


// After Building the Application:
// Configure HTTP request pipline. Apply Use function or Map function methods in order to configure the HTTP request lifecycle.

app.MapCarter();//it maps the routes defined in the ICarterModule implementation



app.Run();
