using Business.DTOS;
using Business.Interfaces;
using Business.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController(ITicketService ticketService) : ControllerBase
    {
        private readonly ITicketService _ticketService = ticketService;

        [HttpPost]
        public async Task<IActionResult> Create(CreateTickets ticket)
        {
            if (!ModelState.IsValid)
                return BadRequest(ticket);

            var result = await _ticketService.CreateAsync(ticket);
            if (result == null)
                return BadRequest("Failed to create ticket.");

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketService.GetAllAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateTickets ticket)
        {
            if (!ModelState.IsValid)
                return BadRequest(ticket);

            var updated = await _ticketService.UpdateAsync(id, ticket);
            if (!updated)
                return NotFound();

            return Ok(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _ticketService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return Ok();
        }
    }
}
