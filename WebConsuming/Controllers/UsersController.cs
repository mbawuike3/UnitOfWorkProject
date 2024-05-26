using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebConsuming.Models;

namespace WebConsuming.Controllers
{
    public class UsersController : Controller
    {
        HttpClient client;
        Uri baseAddress = new Uri("http://localhost:5107/api");
        public UsersController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            List<UsersViewModel> users = new List<UsersViewModel>();
            HttpResponseMessage response = client.GetAsync(baseAddress+ "/Users").Result;
            if(response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<UsersViewModel>>(data)!;
            }
            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(UsersViewModel model)
        {
            var data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(baseAddress+ "/Users", content).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");   
            }
            return View();
        }
        public IActionResult Edit(Guid id)
        {
            UsersViewModel users = new UsersViewModel();
            HttpResponseMessage response = client.GetAsync(baseAddress + "/Users/"+id).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<UsersViewModel>(data)!;
            }
            return View(users);
        }
        [HttpPost]
        public IActionResult Edit(UsersViewModel model)
        {
            var data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(baseAddress + "/Users/"+model.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public IActionResult Details(Guid id)
        {
            UsersViewModel users = new UsersViewModel();
            HttpResponseMessage response = client.GetAsync(baseAddress + "/Users/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<UsersViewModel>(data)!;
            }
            return View(users);
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            UsersViewModel users = new UsersViewModel();
            HttpResponseMessage response =await client.GetAsync(baseAddress + "/Users/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<UsersViewModel>(data)!;
            }
            return View(users);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"{baseAddress}/Users/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to delete the user.");
                }
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while attempting to delete the user.");
            }

            // Re-fetch the user data to show the deletion failed view again
            UsersViewModel users = new UsersViewModel { Id = id };
            HttpResponseMessage fetchResponse = await client.GetAsync($"{baseAddress}/Users/{id}");
            if (fetchResponse.IsSuccessStatusCode)
            {
                var data = await fetchResponse.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<UsersViewModel>(data)!;
            }

            return View("Delete", users); // Return to the Delete view
        }
    }
}
