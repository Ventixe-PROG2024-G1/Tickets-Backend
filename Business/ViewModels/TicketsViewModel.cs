using Domain.Enums;

namespace Business.ViewModels;

public class TicketsViewModel
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsStanding { get; set; }
    public TicketTier Tiers { get; set; }
    //public string Tier => Tier.ToString();  //hmmm
    public string PlacementType => IsStanding ? "Standing" : "Seated";

}
