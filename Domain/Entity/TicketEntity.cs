using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;
public class TicketEntity
{
    [Key]
    public Guid Id { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
   
    public int Quantity { get; set; }
    public string TierDescription { get; set; } = null!;
    public TicketTier Tier { get; set; }

    public Guid EventId { get; set; }
}
