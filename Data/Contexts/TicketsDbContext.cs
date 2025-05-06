using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class TicketsDbContext(DbContextOptions<TicketsDbContext> options) : DbContext(options)
    {
        public DbSet<TicketEntity> TicketSet { get; set; }
    }

}
