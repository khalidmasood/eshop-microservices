using MediatR;
namespace BuildingBlocks.CQRS
{
    //We define 2 ICommandHandler interfaces


    //1) Do not return a response
    //We create the nullable or void response Command handler
    public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>// for null resonse
        where TCommand : ICommand<Unit>
    { 
    }

    //2) returns a response
    //When implementing this, there will be error that TCommand cannot be used as type parameter TRequest, because these comes from the IRequest<IResponse> object 
    //So we should apply the filter "where TCommand : ICommand<TResponse>", which implements the IRequest<IResponse>
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : notnull
    {
    }
}
