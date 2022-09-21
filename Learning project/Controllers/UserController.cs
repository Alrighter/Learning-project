using Learning_project.Interfaces;
using Learning_project.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Learning_project.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var runners = await _userRepository.GetAllUsers();
            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var runner in runners)
            {
                var userViewModel = new UserViewModel()
                {
                    Id = runner.Id,
                    UserName = runner.UserName,
                    Mileage = runner.Milage,
                    Pace = runner.Pace,
                    ProfileImageUrl = runner.ProfileUserUrl,
                };
                users.Add(userViewModel);
            }
            return View(users);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserById(id);
            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Mileage = user.Milage,
                Pace = user.Pace,
            };
            return View(userDetailViewModel);
        }
    }
}
