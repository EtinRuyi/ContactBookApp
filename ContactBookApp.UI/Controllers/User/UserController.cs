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
            if (ModelState.IsValid)
            {
                try
                {
                    using var client = _httpClientFactory.CreateClient();
                    var httpRequestMessage = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri("https://localhost:7083/api/User/AddNewUser/Add-new-user"),
                        Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
                    };
                    var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                    httpResponseMessage.EnsureSuccessStatusCode();
                    var response = await httpResponseMessage.Content.ReadFromJsonAsync<GetUserDto>();

                    if (response != null)
                    {
                        // User added successfully, response contains user information

                        // Capture the user ID
                        var userId = response.Id;

                        // Check if an image was uploaded
                        if (model.UserImage != null && model.UserImage.Length > 0)
                        {
                            var imageUploadResponse = await UploadUserImage(model.UserImage, userId);

                            if (imageUploadResponse == null)
                            {
                                ViewBag.ImageUploadError = "Failed to upload the image.";
                                return View(model);
                            }
                        }

                        ViewBag.msg = "User submitted successfully.";
                        return RedirectToAction("GetAllUser", "User");
                    }
                    else
                    {
                        ViewBag.msg = "Oops... Something went wrong while adding the user.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.msg = "Failed to add the user: " + ex.Message;
                }
            }

            // Handle ModelState errors and display the form with validation messages
            return View(model);
        }

        private async Task<string> UploadUserImage(IFormFile image, string userId)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();

                // Create a form data content for image upload
                var imageContent = new StreamContent(image.OpenReadStream());
                imageContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = image.FileName
                };
                var imageFormData = new MultipartFormDataContent();
                imageFormData.Add(imageContent);

                // Upload the image to your API with the user ID and capture the image URL from the API
                var imageUploadResponse = await httpClient.PostAsync($"https://localhost:7083/api/ImageUpload/UploadUserImage/{userId}", imageFormData);

                if (imageUploadResponse.IsSuccessStatusCode)
                {
                    var imageUrl = await imageUploadResponse.Content.ReadAsStringAsync();
                    // Now, 'imageUrl' contains the Cloudinary image URL returned by your API.

                    // Return the image URL to the calling method
                    return imageUrl;
                }
                else
                {
                    ViewBag.ImageUploadError = "Failed to upload the image.";
                    return null;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ImageUploadError = "Failed to upload the image: " + ex.Message;
                return null;
            }
        }




        //[HttpPost]
        //public async Task<IActionResult> AddNewUser(AddNewUserDto model)
        //{
        //    var client = _httpClientFactory.CreateClient();
        //    var httpRequestMessage = new HttpRequestMessage()
        //    {
        //        Method = HttpMethod.Post,
        //        RequestUri = new Uri("https://localhost:7083/api/User/AddNewUser/Add-new-user"),
        //        Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
        //    };
        //    var httpResponseMessage = await client.SendAsync(httpRequestMessage);
        //    httpResponseMessage.EnsureSuccessStatusCode();
        //    var response =  await httpResponseMessage.Content.ReadFromJsonAsync<GetUserDto>();
        //    if (response != null)
        //    {
        //        ViewBag.msg = "Submitted Successfully";
        //        return RedirectToAction("GetAllUser", "User");
        //    }
        //    else
        //    {
        //        ViewBag.msg = "OOPs.....Something went wrong";
        //    }
        //    return View();
        //}


        [HttpGet]
        public async Task<IActionResult> UpdateUserById(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<UpdateUserDto>($"https://localhost:7083/api/User/GetUserById/Get/{id}?ID={id.ToString()}");
            if (response != null)
            {
                return View(response);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserById(Guid id, UpdateUserDto update)
        {
            if (ModelState.IsValid)
            {
                if (update.UserImage != null && update.UserImage.Length > 0)
                {
                    try
                    {
                        var httpClient = _httpClientFactory.CreateClient();

                        var imageContent = new StreamContent(update.UserImage.OpenReadStream());
                        imageContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                        {
                            Name = "file",
                            FileName = update.UserImage.FileName
                        };
                        var imageFormData = new MultipartFormDataContent();
                        imageFormData.Add(imageContent);

                        var imageUploadResponse = await httpClient.PostAsync("https://localhost:7083/api/ImageUpload/UploadUserImage/" + id, imageFormData);

                        if (imageUploadResponse.IsSuccessStatusCode)
                        {
                            var imageUploadResult = await imageUploadResponse.Content.ReadAsStringAsync();
                            // Parse the response or handle accordingly
                        }
                        else
                        {
                            ViewBag.ImageUploadError = "Failed to upload the image.";
                            return View(update);
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ImageUploadError = "Failed to upload the image: " + ex.Message;
                        return View(update);
                    }
                }

                using var client = _httpClientFactory.CreateClient();
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

                return View(update);
            }

            // Handle ModelState errors and display the form with validation messages
            return View(update);
        }



        //[HttpPost]
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



        //[HttpPost]
        //CORRECT UPDATE USER
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

        //        if (httpResponseMessage.IsSuccessStatusCode)
        //        {
        //            var response = await httpResponseMessage.Content.ReadFromJsonAsync<UpdateUserDto>();
        //            if (response != null)
        //            {
        //                return RedirectToAction("GetAllUser", "User");
        //            }
        //        }
        //        else if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
        //        {
        //            ViewBag.Error = "Incorrect User Password";
        //        }
        //        else
        //        {
        //            ViewBag.Error = "An error occurred while updating the user.";
        //        }

        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = "An error occurred: " + ex.Message;
        //        return View();
        //    }
        //}

        //CHAT GBT
        [HttpGet]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<GetUserDto>($"https://localhost:7083/api/User/GetUserById/Get/{id}");

            if (response != null)
            {
                return View(response);
            }

            return View(null);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"https://localhost:7083/api/User/DeleteUser/Delete/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAllUser", "User");
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    ViewBag.Error = "Incorrect User Password";
                }
                else
                {
                    ViewBag.Error = "An error occurred while deleting the user.";
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred: " + ex.Message;
                return View();
            }
        }



        public async Task<IActionResult> SearchUsers()
        {
            return View();
        }

    }
}