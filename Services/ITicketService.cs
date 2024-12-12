namespace TicketingSystem.Services
{
    public interface ITicketService
    {
        Task<int> GetUserTicketCount(string userId);
    }

}
