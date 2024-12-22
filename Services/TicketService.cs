using TicketingSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace TicketingSystem.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetUserTicketCount(string userId)
        {
            return await _context.Tickets.CountAsync(t => t.UserId == userId);
        }

public async Task<bool> DeleteTicketAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
