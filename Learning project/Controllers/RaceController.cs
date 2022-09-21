using System.Diagnostics.Eventing.Reader;
using Learning_project.Data;
using Learning_project.Interfaces;
using Learning_project.Models;
using Learning_project.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learning_project.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }

        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            CreateRaceViewModel raceViewModel = new CreateRaceViewModel() {AppUserId = curUserId};
            return View(raceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = raceVM.AppUserId,
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        State = raceVM.Address.State,
                        City = raceVM.Address.City,
                    }
                };
                _raceRepository.Add(race);

                return RedirectToAction("Index");
            }
            else
            {
                return View(raceVM);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null)
            {
                return View("Error");
            }

            var raceVM = new EditRaceViewModel()
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit a race.");
                return View(nameof(Edit), model);
            }
            var userRace = await _raceRepository.GetByIdAsyncNoTracking(id);

            if (model != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userRace.Image);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Failed to delete photo.");
                    return View(model);
                }

                var photoResult = await _photoService.AddPhotoAsync(model.Image);

                var race = new Race()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    AddressId = model.AddressId,
                    Address = model.Address,
                    Image = photoResult.Url.ToString(),
                };

                _raceRepository.Update(race);
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var race = _raceRepository.GetByIdAsync(id);
            if (race == null)
            {
                return View("Error");
            }

            return View(race);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            _raceRepository.Delete(race);
            return RedirectToAction(nameof(Index));
        }
    }
}
