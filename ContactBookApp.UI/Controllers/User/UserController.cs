using ContactBookApp.UI.Models.UserDTO;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace ContactBookApp.UI.Controllers.User
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            List<GetUserDto> users = new List<GetUserDto>();
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7083/api/User/GetAllUser/Get-All-Users");
                httpResponseMessage.EnsureSuccessStatusCode();
                users.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<GetUserDto>>()); 
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(users);
        }

       
        public async Task<IActionResult> AddNewUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser(AddNewUserDto model)
        {
            var client = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7083/api/User/AddNewUser/Add-new-user"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response =  await httpResponseMessage.Content.ReadFromJsonAsync<GetUserDto>();
            if (response != null)
            {
                return RedirectToAction("GetAllUser", "User");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUserById(Guid id)
        {
            return View();
        }
    }
}
