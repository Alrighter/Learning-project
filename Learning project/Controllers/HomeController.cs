﻿using Learning_project.Helpers;
using Learning_project.Interfaces;
using Learning_project.Models;
using Learning_project.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace Learning_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClubRepository _clubRepository;

        public HomeController(ILogger<HomeController> logger, IClubRepository clubRepository)
        {
            _logger = logger;
            _clubRepository = clubRepository;
        }

        public async Task<IActionResult> Index()
        {
            var ipInfo = new IPInfo();
            var homeViewModel = new HomeViewModel();
            try
            {
                string url = "https://ipinfo.io/31.148.23.210?token=08393c22d0cf72";
                var info = new WebClient().DownloadString(url);
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
                RegionInfo regionInfo = new RegionInfo(ipInfo.Country);
                ipInfo.Country = regionInfo.EnglishName;
                homeViewModel.City = ipInfo.City;
                homeViewModel.State = ipInfo.Region;
                if (homeViewModel.City != null)
                {
                    homeViewModel.Clubs = await _clubRepository.GetClubByCity(homeViewModel.City);
                }
                else
                {
                    homeViewModel.Clubs = null;
                }
                return View(homeViewModel);
            }
            catch(Exception)
            {
                homeViewModel.Clubs = null;
            }
            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}