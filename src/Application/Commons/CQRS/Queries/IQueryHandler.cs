using System.Threading.Tasks;

namespace Application.Commons.CQRS.Queries
{
    public interface IQueryHandler<RequestedQuery, Query> where Query : IQuery<RequestedQuery>
    {
        Task<RequestedQuery> HandleAsync(Query query);
    }
}
