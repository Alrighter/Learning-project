using CloudinaryDotNet.Actions;
using Learning_project.Data;
using Learning_project.Interfaces;
using Learning_project.Models;
using Learning_project.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Learning_project.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }

        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, ImageUploadResult imageUploadResult)
        {

            user.Id = editVM.Id;
            user.Pace = editVM.Pace;
            user.Milage = editVM.Mileage;
            user.ProfileUserUrl = imageUploadResult.Url.ToString();
            user.State = editVM.State;
            user.City = editVM.City;

        }

        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRepository.GetAllRaces();
            var userClubs = await _dashboardRepository.GetAllClubs();

            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs,
            };

            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUser = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUser);
            if (user == null)
            {
                return View("Error");
            }

            var editUser = new EditUserDashboardViewModel()
            {
                Id = curUser,
                Pace = user.Pace,
                Mileage = user.Milage,
                City = user.City,
                State = user.State,
                ProfileImageUrl = user.ProfileUserUrl
            };
            return View(editUser);


        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile.");
                return View("EditUserProfile", editVM);
            }

            AppUser user = await _dashboardRepository.GetUserByIdNoTracking(editVM.Id);

            if (user.ProfileUserUrl == "" || user.ProfileUserUrl == null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _photoService.DeletePhotoAsync(user.ProfileUserUrl.ToString());
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Could not delete photo.");
                return View(editVM);
            }

            var imagePhotoResult = await _photoService.AddPhotoAsync(editVM.Image);
            MapUserEdit(user, editVM, imagePhotoResult);
            _dashboardRepository.Update(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
