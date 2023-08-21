using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TranslateGPT.Models;

namespace TranslateGPT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly List<string> mostUsedLanguages = new List<string>()
        {
            "English",
            "Mandarin Chinese",
            "Hindi",
            "Spanish",
            "French",
            "Arabic",
            "Bengali",
            "Russian",
            "Portuguese",
            "Urdu",
            "Indonesian",
            "German",
            "Japanese",
            "Swahili",
            "Zulu",
            "Punjabi"

        };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}