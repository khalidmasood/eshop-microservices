var builder = WebApplication.CreateBuilder(args);


// Before Building the Application:
// Add services to the DI container. Registration operations.




var app = builder.Build();



// After Building the Application:
// Configure HTTP request pipline. Apply Use function or Map function methods in order to configure the HTTP request lifecycle.



app.Run();
