using TicketingSystem.Data;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Services;

public class TicketService : ITicketService
{
    private readonly ApplicationDbContext _context;

    public TicketService(ApplicationDbContext context)
    {
        _context = context;
    }

    // تغییر سطح دسترسی به public
    public async Task<int> GetUserTicketCount(string userId)
    {
        // فرض می‌کنیم که تیکت‌ها در جدولی به نام Tickets ذخیره می‌شوند
        return await _context.Tickets.CountAsync(t => t.UserId == userId);
    }
}
