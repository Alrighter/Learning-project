using Learning_project.Data;
using Learning_project.Interfaces;
using Learning_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Learning_project.Repository
{
    public class RaceRepository : IRaceRepository

    {
        private readonly ApplicationDbContext _context;

        public RaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await _context.Races
                .Include(a => a.Address)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Race> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Races
                .Include(a => a.Address)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Race>> GetRaceByCity(string city)
        {
            return await _context.Races
                .Include(a => a.Address)
                .Where(a => a.Address.City == city).ToListAsync();
        }

        public bool Add(Race race)
        {
            _context.Races.Add(race);
            return Save();
        }

        public bool Update(Race race)
        {
            _context.Races.Update(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Races.Remove(race);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
