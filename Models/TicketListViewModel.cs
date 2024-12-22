namespace TicketingSystem.Models
{
    public class TicketListViewModel
    {
        public List<Ticket> Tickets { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public string SearchQuery { get; set; }
    }

}
