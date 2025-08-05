using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors
{
    public class ValidationBehaviors<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : CQRS.ICommand<TResponse>
    {
        //1st parameter represents the incoming request from the client and the next is the next delegate method or the actual behavior.
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            var context = new ValidationContext<TRequest>(request);//validation context for the incoming request

            //and then we run the validators and ofcourse this is async now and we use the Task, and return back the validator results

            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults.Where(x => x.Errors.Any()).SelectMany(r => r.Errors).ToList();

            if (failures.Any())
                throw new ValidationException(failures);

            return await next();// this is important as we need to run the next command in the pipeline
        }
    }

}

