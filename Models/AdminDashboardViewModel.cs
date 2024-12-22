using System.Collections.Generic;

namespace TicketingSystem.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalTickets { get; set; }
        public int TotalResponses { get; set; }
        public int HighPriorityTickets { get; set; }
        public int MediumPriorityTickets { get; set; }
        public int LowPriorityTickets { get; set; }
        public List<PriorityCount> PriorityCounts { get; set; }
        public List<StatusCount> StatusCounts { get; set; }
    }

    public class PriorityCount
    {
        public string Priority { get; set; }
        public int Count { get; set; }
    }

    public class StatusCount
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }
}
