using ContactBook.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.Controllers.User
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        //GetAllUserActionMethod
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<UserDTO> response = new List<UserDTO>();

            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7083/api/user/Get-All-Users");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<UserDTO>>());

            }
            catch (Exception ex)
            {

            }

            return View(response);
        }
    }
}
