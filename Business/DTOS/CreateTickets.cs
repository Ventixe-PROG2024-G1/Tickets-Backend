using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOS;

public class CreateTickets
{
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public string TierDescription { get; set; } = string.Empty;
    [Required]
    public TicketTier Tier { get; set; }
    
    public string? EventId { get; set; }
}
