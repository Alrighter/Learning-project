using Learning_project.Models;

namespace Learning_project.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Remove(AppUser user);
        bool Save();
    }
}
