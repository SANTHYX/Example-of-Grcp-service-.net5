using System.Threading.Tasks;

namespace Application.Commons.CQRS.Queries
{
    public interface IQueryDispatcher
    {
        Task<RequestedQuery> SendAsync<RequestedQuery, Query>(Query query) where Query : IQuery<RequestedQuery>;
    }
}
