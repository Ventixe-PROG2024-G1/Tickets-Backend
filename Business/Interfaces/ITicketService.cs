using Business.DTOS;
using Business.ViewModels;

namespace Business.Interfaces
{
    public interface ITicketService
    {
        Task<TicketsViewModel?> CreateAsync(CreateTickets dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<TicketsViewModel>> GetAllAsync();
        Task<TicketsViewModel?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Guid id, CreateTickets dto);

    }
}