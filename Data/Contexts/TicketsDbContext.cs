using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class TicketsDbContext(DbContextOptions<TicketsDbContext> options) : DbContext(options)
    {
        public DbSet<TicketEntity> TicketSet { get; set; }


        // Ai genererad kod
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<TicketEntity>()
                .Property(t => t.Tier)
                .HasConversion<string>();
        }

    }

}
