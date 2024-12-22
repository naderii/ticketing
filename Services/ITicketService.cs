using System.Threading.Tasks;

namespace TicketingSystem.Services
{
    public interface ITicketService
    {
        // Method to get the number of tickets for a user
        Task<int> GetUserTicketCount(string userId);

        // Method to delete a ticket (asynchronous)
        Task<bool> DeleteTicketAsync(int ticketId);
    }
}
