using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using TranslateGPT.DTOs;
using TranslateGPT.Models;

namespace TranslateGPT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

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

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            ViewBag.GPTLanguages = new SelectList(mostUsedLanguages);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OpenAiGPTResponse(string query, string selectedLanguage)
        {
            //Get openApi key from appsettings.json, inject iconfiguratoin
            var openApiKey = _configuration["OpenAI:ApiKey"];

            //set up the http client with open ai key
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openApiKey}");

            //define the request payload
            var payload = new
            {
                //model to be used, GPT-4 model will be used. 
                model = "gpt-4",
                //array of objects that reprsents a conversation
                //between user and system
                messages = new object[]
                {
                    //instructs the model to translate to language indicated by selectedlanguage
                    new{ role = "system", content = $"Translate to {selectedLanguage}" },
                    //this is user inputs query
                    new{ role = "user", content= query}

                },
                //controls randomness of the generated text, how long or 
                //short the generated response will be in regards to parameters.
                temperature = 0,
                //max tokens or units of text that generated output should have.
                max_tokens = 256
            };

            //the payload is then serialised to into JSON string and wrapped in HTTP content
            //object for sending to an API, as a POST request.
            string jsonPayload = JsonConvert.SerializeObject(payload);
            HttpContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            //send the request
            var responseMessage = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", httpContent);           
            var responseMessageJson = await responseMessage.Content.ReadAsStringAsync();


            //return the response
            var response = JsonConvert.DeserializeObject<OpenAIResponse>(responseMessageJson);
            ViewBag.Result = response.Choices[0].Message.Content;
            ViewBag.Languages = new SelectList(mostUsedLanguages);

            return View("Index");

        }

    }
}