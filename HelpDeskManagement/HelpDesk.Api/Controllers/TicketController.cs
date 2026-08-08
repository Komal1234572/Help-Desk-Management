using Microsoft.AspNetCore.Mvc;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;

namespace HelpDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        // GET: api/Ticket/All
        [HttpGet("All")]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _ticketRepository.GetAllTicketsAsync();
            return Ok(tickets);
        }

        // GET: api/Ticket/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound($"Ticket with Id {id} was not found.");
            }

            return Ok(ticket);
        }

        // POST: api/Ticket
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
        {
            if (ticket == null)
            {
                return BadRequest("Ticket data is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newId = await _ticketRepository.CreateTicketAsync(ticket);
            ticket.Id = newId;

            return CreatedAtAction(nameof(GetTicketById), new { id = newId }, ticket);
        }

        // PUT: api/Ticket/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] Ticket ticket)
        {
            if (ticket == null || id != ticket.Id)
            {
                return BadRequest("Ticket Id mismatch or invalid data.");
            }

            var existingTicket = await _ticketRepository.GetTicketByIdAsync(id);
            if (existingTicket == null)
            {
                return NotFound($"Ticket with Id {id} was not found.");
            }

            await _ticketRepository.UpdateTicketAsync(ticket);
            return Ok(ticket);
        }

        // DELETE: api/Ticket/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var existingTicket = await _ticketRepository.GetTicketByIdAsync(id);
            if (existingTicket == null)
            {
                return NotFound($"Ticket with Id {id} was not found.");
            }

            await _ticketRepository.DeleteTicketAsync(id);
            return Ok($"Ticket with Id {id} was deleted successfully.");
        }

        // GET: api/Ticket/Status/{status}
        [HttpGet("Status/{status}")]
        public async Task<IActionResult> GetTicketsByStatus(string status)
        {
            var tickets = await _ticketRepository.GetTicketsByStatusAsync(status);
            return Ok(tickets);
        }
    }
}
