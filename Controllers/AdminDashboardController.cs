using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TicketingSystem.Data;
using TicketingSystem.Models;
using TicketingSystem.ViewModels;

namespace TicketingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalTickets = _context.Tickets.Count();
            var totalResponses = _context.TicketResponses.Count();

            var highPriorityTickets = _context.Tickets.Count(t => t.Priority == "High");
            var mediumPriorityTickets = _context.Tickets.Count(t => t.Priority == "Medium");
            var lowPriorityTickets = _context.Tickets.Count(t => t.Priority == "Low");

            var priorityCounts = _context.Tickets
                .GroupBy(t => t.Priority)
                .Select(g => new PriorityCount
                {
                    Priority = g.Key,
                    Count = g.Count()
                }).ToList();

            var statusCounts = _context.Tickets
                .GroupBy(t => t.Status)
                .Select(g => new StatusCount
                {
                    Status = g.Key,
                    Count = g.Count()
                }).ToList();

            var model = new AdminDashboardViewModel
            {
                TotalTickets = totalTickets,
                TotalResponses = totalResponses,
                HighPriorityTickets = highPriorityTickets,
                MediumPriorityTickets = mediumPriorityTickets,
                LowPriorityTickets = lowPriorityTickets,
                PriorityCounts = priorityCounts,
                StatusCounts = statusCounts
            };

            return View(model);
        }
    }
}
