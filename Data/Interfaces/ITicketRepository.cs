using Domain.Entity;
using System.Linq.Expressions;

namespace Data.Repository
{
    public interface ITicketRepository
    {
        Task<bool> AddAsync(TicketEntity entity);
        Task<bool> DeleteAsync(Expression<Func<TicketEntity, bool>> expression);
        Task<IEnumerable<TicketEntity>> GetAllAsync(bool sortByDescending = false, Expression<Func<TicketEntity, object>>? sortBy = null, Expression<Func<TicketEntity, bool>>? filterBy = null, params Expression<Func<TicketEntity, object>>[] includes);
        Task<IEnumerable<TicketEntity>> GetByEventIdAsync(Guid eventId);
        Task<TicketEntity?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(TicketEntity entity);
    }
}