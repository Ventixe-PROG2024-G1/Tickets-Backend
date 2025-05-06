using Business.DTOS;
using Business.Interfaces;
using Business.ViewModels;
using Data.Contexts;
using Data.Repository;
using Domain.Entity;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services;

public class TicketService(ITicketRepository repository, IMemoryCache cache) : ITicketService
{
    private readonly ITicketRepository _ticketRepository = repository;
    private readonly IMemoryCache _cache = cache;
    private const string _cacheKey_All = "Ticket_All";

    public async Task<IEnumerable<TicketsViewModel>> GetAllAsync()
    {
        if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<TicketsViewModel>? cachedTickets))
            return cachedTickets!;

        return await SetCache();



        //var tickets = await _ticketRepository.GetAllAsync();
        //return tickets.Select(x => MapToViewModel(x));


    }

    public async Task<TicketsViewModel?> GetByIdAsync(Guid id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        return ticket is null ? null : MapToViewModel(ticket);
    }

    public async Task<TicketsViewModel?> CreateAsync(CreateTickets ticket)
    {
        var entity = new TicketEntity
        {
            Id = Guid.NewGuid(),
            Price = ticket.Price,
            Quantity = ticket.Quantity,
            IsStanding = ticket.IsStanding,
            Tier = ticket.Tier,
            //EventId = dto.EventId
        };

        var result = await _ticketRepository.AddAsync(entity);
        if (result)
        {
            await SetCache(); 
            return MapToViewModel(entity);
        }
        return null;
        //return result ? MapToViewModel(entity) : null;
    }



    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _ticketRepository.DeleteAsync(x => x.Id == id);
        if (result)
            await SetCache();
        return result;
    }



    public async Task<bool> UpdateAsync(Guid id, CreateTickets ticket)
    {
        var existingTicket = await _ticketRepository.GetByIdAsync(id);
        if (existingTicket == null)
            return false;

        existingTicket.Price = ticket.Price;
        existingTicket.Quantity = ticket.Quantity;
        existingTicket.IsStanding = ticket.IsStanding;
        existingTicket.Tier =   ticket.Tier;
        //existingTicket.EventId = dto.EventId;

        var result = await _ticketRepository.UpdateAsync(existingTicket);
        if (result)
            await SetCache();
        return result;
    }



    private static TicketsViewModel MapToViewModel(TicketEntity entity)
    {
        return new TicketsViewModel
        {
            Id = entity.Id,
            Price = entity.Price,
            Quantity = entity.Quantity,
            IsStanding = entity.IsStanding,
            Tiers = entity.Tier
        };
    }


    public async Task<IEnumerable<TicketsViewModel>> SetCache()
    {
        _cache.Remove(_cacheKey_All);
        var entites = await _ticketRepository.GetAllAsync(sortBy: x => x.Id);
        var tickets = entites.Select(entity => new TicketsViewModel
        {
            Id = entity.Id,
            Price = entity.Price,
            Quantity = entity.Quantity,
            IsStanding = entity.IsStanding,
            Tiers = entity.Tier 
        });

        _cache.Set(_cacheKey_All, tickets, TimeSpan.FromMinutes(30));
        return tickets;
    }
}
