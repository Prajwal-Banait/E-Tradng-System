using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETradingSystem1.EFCore;
using ETradingSystem1.Entities;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using ETradingSystem1.Mvc;

//using ETradingSystem1.EFCore;

namespace ETradingSystem1.MVC.Controllers
{
    public class ShareMVCController : Controller
    {
        // display all Share
        public async Task<IActionResult> Index()
        {
            List<Share> objShare = await GetShares();

            return View(objShare);
        }

        // display perticular Share details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var share = await GetShares(id);

                if (share == null)
                {
                    return NotFound();
                }

                return View(share);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // enter new Share data
        public IActionResult Create()
        {
            return View();
        }

        // bind enter data to create new record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShareId,SharesName,SharePrice,ShareQuantity")] Share share)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await AddShare(share);
                }
                catch (Exception)
                {
                    if (ShareExists(share.ShareId) == null)
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

            return View(share);
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
                var share = await GetShares(id);
                if (share == null)
                {
                    return NotFound();
                }

                return View(share);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //  bind enter data 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShareId,SharesName,SharePrice,ShareQuantity")] Share share)
        {
            if (id != share.ShareId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateShare(share);

                }
                catch (Exception)
                {
                    if (ShareExists(share.ShareId) == null)
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

            return View(share);
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
                Share objShare = await GetShares(id);

                if (objShare == null)
                {
                    return NotFound();
                }

                return View(objShare);
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

            await DeleteShare(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<Share> ShareExists(int id)
        {

            Share objshare = await GetShares(id);
            return objshare;

        }




        //----------Separate function used by actions methods of our controller

        // get all Share data from server side
        public async Task<List<Share>> GetShares()
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

                HttpResponseMessage response = await client.GetAsync("/api/ShareWebAPI");
                string stringData = response.Content.ReadAsStringAsync().Result;
                List<Share> data = JsonConvert.DeserializeObject<List<Share>>(stringData);

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

        // get perticular Share from server side
        public async Task<Share> GetShares(int? id)
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

                HttpResponseMessage response = await client.GetAsync("/api/ShareWebAPI/" + id);
                string stringData = response.Content.ReadAsStringAsync().Result;
                Share data = JsonConvert.DeserializeObject<Share>(stringData);

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
        public async Task<Share> AddShare(Share share)
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

                string stringData = JsonConvert.SerializeObject(share);
                var contentData = new StringContent(stringData,
            System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/ShareWebAPI/", contentData);

                if (response.IsSuccessStatusCode)
                {
                    
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Unauthorized!";
                }
                return share;
            }
            catch (Exception)
            {
                throw;
            }

        }

        // send edited data to server side
        public async Task<Share> UpdateShare(Share share)
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

                string stringData = JsonConvert.SerializeObject(share);
                var contentData = new StringContent(stringData,
            System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("api/shareWebAPI/" + share.ShareId, contentData);

                if (response.IsSuccessStatusCode)
                {
                   
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Unauthorized!";
                }
                return share;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // send request to server side to delete record
        public async Task<Share> DeleteShare(int? id)
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

                HttpResponseMessage response = await client.DeleteAsync("/api/ShareWebApi/" + id);
                string stringData = response.Content.
            ReadAsStringAsync().Result;
                Share data = JsonConvert.DeserializeObject<Share>(stringData);

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
