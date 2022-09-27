using Microsoft.AspNetCore.Mvc;
using ETradingSystem1.Entities;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using ETradingSystem1.Mvc;

namespace ETradingSystem1.MVC.Controllers
{
    public class BusinessOwnerMVCController : Controller
    {
        // display all BusinessOwner
        public async Task<IActionResult> Index()
        {
            List<BusinessOwner> objBusinessOwner = await GetBusinessOwners();

            return View(objBusinessOwner);
        }

        // display perticular BusinessOwner details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var businessOwner = await GetBusinessOwners(id);

                if (businessOwner == null)
                {
                    return NotFound();
                }

                return View(businessOwner);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // enter new BusinessOwner data
        public IActionResult Create()
        {
            return View();
        }

        // bind enter data to create new record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BusinessOwnerId,Name,Contact,MailId")] BusinessOwner businessOwner)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await AddBusinessOwner(businessOwner);
                }
                catch (Exception)
                {
                    if (BusinessOwnerExists(businessOwner.BusinessOwnerId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(businessOwner);
        }

        // edit perticular data
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            try
            {
                var businessOwner = await GetBusinessOwners(id);
                if (businessOwner == null)
                {
                    return NotFound();
                }

                return View(businessOwner);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //  bind enter data 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BusinessOwnerId,Name,Contact,MailId")] BusinessOwner businessOwner)
        {
            if (id != businessOwner.BusinessOwnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateBusinessOwner(businessOwner);

                }
                catch (Exception)
                {
                    if (BusinessOwnerExists(businessOwner.BusinessOwnerId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(businessOwner);
        }

        // delete perticular data
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                BusinessOwner objBusinessOwner = await GetBusinessOwners(id);

                if (objBusinessOwner == null)
                {
                    return NotFound();
                }

                return View(objBusinessOwner);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // this function give confirmation 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            await DeleteBusinessOwner(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<BusinessOwner> BusinessOwnerExists(int id)
        {

            BusinessOwner objbusinessOwner = await GetBusinessOwners(id);
            return objbusinessOwner;

        }




        //----------Separate function used by actions methods of our controller

        // get all BusinessOwner data from server side
        public async Task<List<BusinessOwner>> GetBusinessOwners()
        {
            try
            {
                string baseUrl = "https://localhost:7056";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var token = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync("/api/BusinessOwnerWebApi");
                string stringData = response.Content.ReadAsStringAsync().Result;
                List<BusinessOwner> data = JsonConvert.DeserializeObject<List<BusinessOwner>>(stringData);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Unauthorized!";
                }
                else
                {
                    return data;
                }

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // get perticular BusinessOwner from server side
        public async Task<BusinessOwner> GetBusinessOwners(int? id)
        {
            try
            {
                string baseUrl = "https://localhost:7056";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var token = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync("/api/BusinessOwnerWebApi/" + id);
                string stringData = response.Content.ReadAsStringAsync().Result;
                BusinessOwner data = JsonConvert.DeserializeObject<BusinessOwner>(stringData);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Unauthorized!";
                }
                else
                {

                    return data;
                }

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // send enter data to server side
        public async Task<BusinessOwner> AddBusinessOwner(BusinessOwner businessOwner)
        {
            try
            {
                string baseUrl = "https://localhost:7056";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var token = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string stringData = JsonConvert.SerializeObject(businessOwner);
                var contentData = new StringContent(stringData,
            System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/BusinessOwnerWebApi/", contentData);

                if (response.IsSuccessStatusCode)
                {
                    
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Unauthorized!";
                }
                return businessOwner;
            }
            catch (Exception)
            {
                throw;
            }

        }

        // send edited data to server side
        public async Task<BusinessOwner> UpdateBusinessOwner(BusinessOwner businessOwner)
        {
            try
            {
                string baseUrl = "https://localhost:7056";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                var contentType = new MediaTypeWithQualityHeaderValue
            ("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var token = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string stringData = JsonConvert.SerializeObject(businessOwner);
                var contentData = new StringContent(stringData,
            System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("api/businessOwnerWebApi/" + businessOwner.BusinessOwnerId, contentData);

                if (response.IsSuccessStatusCode)
                {
                   
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Unauthorized!";
                }
                return businessOwner;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // send request to server side to delete record
        public async Task<BusinessOwner> DeleteBusinessOwner(int? id)
        {
            try
            {
                string baseUrl = "https://localhost:7056";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                var contentType = new MediaTypeWithQualityHeaderValue
            ("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var token = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.DeleteAsync("/api/BusinessOwnerWebApi/" + id);
                string stringData = response.Content.
            ReadAsStringAsync().Result;
                BusinessOwner data = JsonConvert.DeserializeObject<BusinessOwner>(stringData);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Unauthorized!";
                }
                else
                {

                    return data;
                }

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
