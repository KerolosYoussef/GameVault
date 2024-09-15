using MediatR;

namespace GameVault.Common.Interfaces.CQRS.Commands
{
    public interface ICommand : ICommand<Unit> { }
    public interface ICommand<out TResponse> : IRequest<TResponse> { }
}
