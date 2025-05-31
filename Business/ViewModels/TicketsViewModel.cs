using Domain.Enums;

namespace Business.ViewModels;

public class TicketsViewModel
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public TicketTier Tiers { get; set; }
    public string TierDescription { get; set; } = null!;
    public Guid EventId { get; set; }

}
