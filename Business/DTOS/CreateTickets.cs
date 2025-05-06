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
    public bool IsStanding { get; set; }
    [Required]
    public TicketTier Tier { get; set; }
    //[Required]
    public Guid? EventId { get; set; }
}
