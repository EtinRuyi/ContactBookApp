using ContactBookApp.UI.Models.UserDTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;
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
                ViewBag.msg = "Submitted Successfully";
                return RedirectToAction("GetAllUser", "User");
            }
            else
            {
                ViewBag.msg = "OOPs.....Something went wrong";
            }
            return View();
        }

        
        [HttpGet]
        public async Task<IActionResult> UpdateUserById(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<UpdateUserDto>($"https://localhost:7083/api/User/GetUserById/Get/{id}?ID={id.ToString()}");
            if(response != null)
            {
                return View(response);
            }
            return View(null);
        }

        [HttpPost]
        //public async Task<IActionResult> UpdateUserById(Guid id, UpdateUserDto update)
        //{
        //    try
        //    {
        //        var client = _httpClientFactory.CreateClient();
        //        var httpRequestMessage = new HttpRequestMessage()
        //        {
        //            Method = HttpMethod.Put,
        //            RequestUri = new Uri($"https://localhost:7083/api/User/UpdateUserById/Update/{id}?ID={id}"),
        //            Content = new StringContent(JsonSerializer.Serialize(update), Encoding.UTF8, "application/json"),
        //        };
        //        var httpResponseMessage = await client.SendAsync(httpRequestMessage);
        //        httpResponseMessage.EnsureSuccessStatusCode();
        //        var response = await httpResponseMessage.Content.ReadFromJsonAsync<UpdateUserDto>();
        //        if (response != null)
        //        {
        //            return RedirectToAction("GetAllUser", "User");
        //        }
        //        else
        //        {
        //            ViewBag.msg = "Incorrect User Password";
        //        }
        //        return View();
        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception("OOPs.....Something went wrong " );
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> UpdateUserById(Guid id, UpdateUserDto update)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7083/api/User/UpdateUserById/Update/{id}?ID={id}"),
                    Content = new StringContent(JsonSerializer.Serialize(update), Encoding.UTF8, "application/json"),
                };
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var response = await httpResponseMessage.Content.ReadFromJsonAsync<UpdateUserDto>();
                    if (response != null)
                    {
                        return RedirectToAction("GetAllUser", "User");
                    }
                }
                else if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
                {
                    ViewBag.Error = "Incorrect User Password";
                }
                else
                {
                    ViewBag.Error = "An error occurred while updating the user.";
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid id)
        {

            return View();
        }

    }
}