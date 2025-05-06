using Data.Contexts;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using System.Diagnostics;

namespace Data.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketsDbContext _context;

        public TicketRepository(TicketsDbContext context)
        {
            _context = context;
        }

        public async Task<TicketEntity?> GetByIdAsync(Guid id)
        {
            return await _context.TicketSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TicketEntity>> GetAllAsync(bool sortByDescending = false, Expression<Func<TicketEntity, object>>? sortBy = null, Expression<Func<TicketEntity, bool>>? filterBy = null, params Expression<Func<TicketEntity, object>>[] includes)
        {
            IQueryable<TicketEntity> query = _context.TicketSet;

            if (filterBy != null)
                query = query.Where(filterBy);

            if (includes != null && includes.Length != 0)
                foreach (var include in includes)
                    query = query.Include(include);

            if (sortBy != null)
                query = sortByDescending
                    ? query.OrderByDescending(sortBy)
                    : query.OrderBy(sortBy);

            return await query.ToListAsync();
        }


        public virtual async Task<bool> UpdateAsync(TicketEntity entity)
        {
            if (entity == null)
                return false;
            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }



        public virtual async Task<bool> AddAsync(TicketEntity entity)
        {
            if (entity == null)
                return false;
            try
            {
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }


        }




        public virtual async Task<bool> DeleteAsync(Expression<Func<TicketEntity, bool>> expression)
        {
            var entity = await _context.TicketSet.FirstOrDefaultAsync(expression);
            if (entity == null)
                return false;

            try
            {
                _context.TicketSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<IEnumerable<TicketEntity>> GetByEventIdAsync(Guid eventId) // får se hur detta ska su ut egentligen när jag har tillång till andras microservices
        {
            return await _context.TicketSet
                .Where(t => t.EventId == eventId)
                .ToListAsync();
        }
    }
}
