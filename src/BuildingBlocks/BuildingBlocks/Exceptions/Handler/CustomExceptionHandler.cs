using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace BuildingBlocks.Exceptions.Handler;
public class CustomExceptionHandler
    (ILogger<CustomExceptionHandler> logger)
    : IExceptionHandler
{


    //In this method our custom handler class will lock the exceptions and determin e the response based on 
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(
            "Error Message: {exceptionMessage}, Time of occurrence {time}",
            exception.Message, DateTime.UtcNow);

        //anonymous object, to implement the that we will impelment these values for each switch statement
        (string Detail, string Title, int StatusCode) details = exception switch
        {
            InternalServerException => //mapping to anonymous fields above for details object
            (
                exception.Message, //Detail
                exception.GetType().Name, //Title
                context.Response.StatusCode = StatusCodes.Status500InternalServerError //StatusCode
            ),
            ValidationException =>
            (
            //all the validation exceptions will go here, so this is status 400 bad request
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            BadRequestException =>
            (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            NotFoundException =>
            (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status404NotFound
            ),
            _ =>
            (
                //default switch, everything mapped to Status500

                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status500InternalServerError
            )
        };

        //create the problemDetails object and populate from the anonymous pobject.
        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = context.Request.Path
        };

        //And after that we can add more custom problem detail as an extension

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if (exception is ValidationException validationException) // and if validation exception, we can add Errors from the validationException
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }
        //now we can write all these problemdetails in a Json format in amore readable and maneagable way.
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}