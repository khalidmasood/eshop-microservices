using MediatR;
namespace BuildingBlocks.CQRS
{

    //So we have defined a contract for all ICommand types

    //an empty or non-generic type of ICommand
    public interface ICommand : ICommand<Unit> //Unit represent the empty or void type of MediatR
    { 
    
    }

    //generic Type ICommand, generic version represents ICommand that produce a response
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
