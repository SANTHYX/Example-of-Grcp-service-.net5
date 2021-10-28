using Application.Commons.CQRS.Queries;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceScopeFactory _factory;

        public QueryDispatcher(IServiceScopeFactory factory)
        {
            _factory = factory;
        }

        public async Task<TResult> SendAsync<TResult, TQuery>(TQuery query) where TQuery : IQuery<TResult>
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query), "Passed query is empty");
            }

            using var scope = _factory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TResult, TQuery>>();
            return await handler.HandleAsync(query);
        }
    }
}
