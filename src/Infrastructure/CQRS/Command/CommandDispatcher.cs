using Application.Commons.CQRS.Command;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceScopeFactory _factory;

        public CommandDispatcher(IServiceScopeFactory factory)
        {
            _factory = factory;
        }

        public async Task DispatchAsync<T>(T command) where T : ICommand
        {
            if (command is null)
            {
                throw new ArgumentNullException( nameof(command), "Passed command is empty");
            }

            using var scope = _factory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<T>>();
            await handler.HandleAsync(command);
        }
    }
}
