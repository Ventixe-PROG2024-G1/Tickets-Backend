using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;
public class TicketEntity
{
    public Guid Id { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public bool IsStanding { get; set; }
    public int Quantity { get; set; }
    public TicketTier Tier { get; set; } //får se hur det där blir när man skapar 

    public Guid EventId { get; set; } //oklart om det ska va såhär
}
