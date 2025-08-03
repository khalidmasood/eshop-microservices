using MediatR;

namespace BuildingBlocks.CQRS
{

    //IQuery interface for our queries
    //returns TResponse so out TResponse,
    //and inherit from MediatR.IRequest<TResponse>
    public interface IQuery<out TResponse> : IRequest<TResponse>
        where TResponse : notnull // and apply this generic filter where TResponse could not be null
    {
    }
}
