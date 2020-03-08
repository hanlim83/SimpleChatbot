using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SimpleChatbot.Models;

namespace SimpleChatbot.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ActionResult> Index()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(
              HttpMethod.Post,
              $" https://directline.botframework.com/v3/directline/tokens/generate");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["DirectLineSecret"]);
            var userId = $"dl_{Guid.NewGuid()}";
            request.Content = new StringContent(
            JsonConvert.SerializeObject(
                new { User = new { Id = userId } }),
                Encoding.UTF8,
                "application/json");
            var response = await client.SendAsync(request);
            string token = String.Empty;
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<DirectLineToken>(body).token;
            }
            var config = new ChatConfig()
            {
                Token = token,
                UserId = userId
            };
            return View(config);
        }
    }
}