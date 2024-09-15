using MediatR;

namespace GameVault.Common.Interfaces.CQRS.Queries
{
    public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull { }
}
