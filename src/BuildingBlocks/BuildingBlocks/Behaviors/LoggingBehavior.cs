using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

//Inherit this class from the IPipelineBheavior
// and this will be including <TRequest, TResponse> generics, for that purpose we will also make the LoggingBehavior as Generics <TRequest, TResponse>.
// and implement that IPipelineBehavior's Handle method
//Before that configure our LoggingBehavior, make it public and inject ILogger,
//Apply filters on TRequest and TResponse, TRequest should not be null and also should inherit from IRequest<TResponse>, 
//And TResponse should not be null
public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{

    // You can see that it is accepting the TRequest as request parameter, the next parameter is the RequestHandleDelegate, calls next pipeline or actual Handle operation.
    // Make it async.
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        
        //Always start with some logging about the Request and the Response of the incomming object so we log the name of request and response and then log the whole request object.
        logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();

        //Then log the duration of the actual Handle or next Pipeline step(s)
        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3) // if the request is greater than 3 seconds, then log the warnings, this is performance concern for the application, a good illustartion of flagging the long running methods in the application.
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
                typeof(TRequest).Name, timeTaken.Seconds);

        logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);
        return response;
    }
}