using System.Threading.Tasks;

namespace Application.Commons.CQRS.Command
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<T>(T command) where T : ICommand;
    }
}
