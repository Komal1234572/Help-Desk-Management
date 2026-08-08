using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            return await _context.Tickets
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<int> CreateTicketAsync(Ticket ticket)
        {
            ticket.CreatedDate = DateTime.Now;
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return ticket.Id;
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            var existingTicket = await _context.Tickets.FindAsync(ticket.Id);
            if (existingTicket == null)
            {
                return;
            }

            existingTicket.Title = ticket.Title;
            existingTicket.Description = ticket.Description;
            existingTicket.Priority = ticket.Priority;
            existingTicket.Status = ticket.Status;
            existingTicket.RaisedBy = ticket.RaisedBy;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return;
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Ticket>> GetTicketsByStatusAsync(string status)
        {
            return await _context.Tickets
                .AsNoTracking()
                .Where(t => t.Status.ToLower() == status.ToLower())
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
        }
    }
}
