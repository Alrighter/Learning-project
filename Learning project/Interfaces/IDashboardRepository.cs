using Learning_project.Models;

namespace Learning_project.Interfaces
{
    public interface IDashboardRepository
    {
        public Task<List<Race>> GetAllRaces();
        public Task<List<Club>> GetAllClubs();
        public Task<AppUser> GetUserById(string id);
        public Task<AppUser> GetUserByIdNoTracking(string id);
        public bool Update(AppUser user);
    }
}
