using Pozdravlyator.Models;

namespace Pozdravlyator.Services
{
    public interface IBirthdayService
    {
        Task<List<Person>> GetAllSortedAsync();
        Task<List<Person>> GetTodayAndUpcomingAsync(int daysAhead = 14);
    }
}