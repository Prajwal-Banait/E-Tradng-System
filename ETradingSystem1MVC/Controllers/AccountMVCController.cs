using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETradingSystem1.EFCore;
using ETradingSystem1.Entities;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using ETradingSystem1.Mvc;

namespace ETradingSystem1.MVC.Controllers
{
    public class AccountMVCController : Controller
    {
        // display all Customer accounts types
        public async Task<IActionResult> Index()
        {
            List<Account> objAccount = await GetAccounts();

            return View(objAccount);
        }

        // display perticular Account details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var account = await GetAccounts(id);

                if (account == null)
                {
                    return NotFound();
                }

                return View(account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // enter new Account data
        public IActionResult Create()
        {
            return View();
        }

        // bind enter data to create new record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,CustomerName,CustomerId,ShareId,ShareName,ShareQuantity,SharePrice,AccountBalance")] Account account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await AddAccount(account);
                }
                catch (Exception)
                {
                    if (AccountExists(account.AccountId) == null)
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

            return View(account);
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
                var account = await GetAccounts(id);
                if (account == null)
                {
                    return NotFound();
                }

                return View(account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //  bind enter data 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,CustomerName,CustomerId,ShareId,ShareName,ShareQuantity,SharePrice,AccountBalance")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateAccount(account);

                }
                catch (Exception)
                {
                    if (AccountExists(account.AccountId) == null)
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

            return View(account);
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
                Account objAccount = await GetAccounts(id);

                if (objAccount == null)
                {
                    return NotFound();
                }

                return View(objAccount);
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

            await DeleteAccount(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<Account> AccountExists(int id)
        {

            Account objaccount = await GetAccounts(id);
            return objaccount;

        }




        //----------Separate function used by actions methods of our controller

        // get all Account data from server side
        public async Task<List<Account>> GetAccounts()
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

                HttpResponseMessage response = await client.GetAsync("/api/AccountWebAPI");
                string stringData = response.Content.ReadAsStringAsync().Result;
                List<Account> data = JsonConvert.DeserializeObject<List<Account>>(stringData);

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

        // get perticular Account from server side
        public async Task<Account> GetAccounts(int? id)
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

                HttpResponseMessage response = await client.GetAsync("/api/AccountWebAPI/" + id);
                string stringData = response.Content.ReadAsStringAsync().Result;
                Account data = JsonConvert.DeserializeObject<Account>(stringData);

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
        public async Task<Account> AddAccount(Account account)
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

                string stringData = JsonConvert.SerializeObject(account);
                var contentData = new StringContent(stringData,
            System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/AccountWebAPI/", contentData);

                if (response.IsSuccessStatusCode)
                {
                   
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Unauthorized!";
                }
                return account;
            }
            catch (Exception)
            {
                throw;
            }

        }

        // send edited data to server side
        public async Task<Account> UpdateAccount(Account account)
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

                string stringData = JsonConvert.SerializeObject(account);
                var contentData = new StringContent(stringData,
            System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("api/accountWebAPI/" + account.AccountId, contentData);

                if (response.IsSuccessStatusCode)
                {
                    
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Unauthorized!";
                }
                return account;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // send request to server side to delete record
        public async Task<Account> DeleteAccount(int? id)
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

                HttpResponseMessage response = await client.DeleteAsync("/api/AccountWebApi/" + id);
                string stringData = response.Content.
            ReadAsStringAsync().Result;
                Account data = JsonConvert.DeserializeObject<Account>(stringData);

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
