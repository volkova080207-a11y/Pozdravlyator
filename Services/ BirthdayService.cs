using Microsoft.EntityFrameworkCore;
using Pozdravlyator.Data;
using Pozdravlyator.Models;

namespace Pozdravlyator.Services
{
    public class BirthdayService : IBirthdayService
    {
        private readonly ApplicationDbContext _db;

        public BirthdayService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Person>> GetAllSortedAsync()
        {
            var people = await _db.People.ToListAsync();
            var today = DateTime.Today;

            return people
                .OrderBy(p => (p.NextOccurrence(today) - today).Days)
                .ToList();
        }

        public async Task<List<Person>> GetTodayAndUpcomingAsync(int daysAhead = 14)
        {
            var people = await _db.People.ToListAsync();
            var today = DateTime.Today;

            return people
                .Select(p => new { Person = p, Days = (p.NextOccurrence(today) - today).Days })
                .Where(x => x.Days >= 0 && x.Days <= daysAhead)
                .OrderBy(x => x.Days)
                .Select(x => x.Person)
                .ToList();
        }
    }
}