using System.Threading.Tasks;

namespace Application.Commons.CQRS.Command
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
