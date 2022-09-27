using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETradingSystem1.EFCore;
using ETradingSystem1.Entities;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using ETradingSystem1.Mvc;

namespace ETradingSystem1.MVC.Controllers
{
    public class CustomerMVCController : Controller
    {
        // display all Customer
        public async Task<IActionResult> Index()
        {
            List<Customer> objCustomer = await GetCustomers();

            return View(objCustomer);
        }

        // display perticular account type details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var customer = await GetCustomers(id);

                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // enter new Customer data
        public IActionResult Create()
        {
            return View();
        }

        // bind enter data to create new record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,Contact,MailId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await AddCustomer(customer);
                }
                catch (Exception)
                {
                    if (CustomerExists(customer.CustomerId) == null)
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

            return View(customer);
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
                var customer = await GetCustomers(id);
                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //  bind enter data 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,Name,Contact,MailId")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateCustomer(customer);

                }
                catch (Exception)
                {
                    if (CustomerExists(customer.CustomerId) == null)
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

            return View(customer);
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
                Customer objCustomer = await GetCustomers(id);

                if (objCustomer == null)
                {
                    return NotFound();
                }

                return View(objCustomer);
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

            await DeleteCustomer(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<Customer> CustomerExists(int id)
        {

            Customer objcustomer = await GetCustomers(id);
            return objcustomer;

        }




        //----------Separate function used by actions methods of our controller

        // get all Customer data from server side
        public async Task<List<Customer>> GetCustomers()
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

                HttpResponseMessage response = await client.GetAsync("/api/CustomerWebAPI");
                string stringData = response.Content.ReadAsStringAsync().Result;
                List<Customer> data = JsonConvert.DeserializeObject<List<Customer>>(stringData);

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

        // get perticular Customer from server side
        public async Task<Customer> GetCustomers(int? id)
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

                HttpResponseMessage response = await client.GetAsync("/api/CustomerWebAPI/" + id);
                string stringData = response.Content.ReadAsStringAsync().Result;
                Customer data = JsonConvert.DeserializeObject<Customer>(stringData);

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
        public async Task<Customer> AddCustomer(Customer customer)
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

                string stringData = JsonConvert.SerializeObject(customer);
                var contentData = new StringContent(stringData,
            System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/CustomerWebAPI/", contentData);

                if (response.IsSuccessStatusCode)
                {
                    
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Unauthorized!";
                }
                return customer;
            }
            catch (Exception)
            {
                throw;
            }

        }

        // send edited data to server side
        public async Task<Customer> UpdateCustomer(Customer customer)
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

                string stringData = JsonConvert.SerializeObject(customer);
                var contentData = new StringContent(stringData,
            System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("api/customerWebAPI/" + customer.CustomerId, contentData);

                if (response.IsSuccessStatusCode)
                {
                   
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Unauthorized!";
                }
                return customer
                    ;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // send request to server side to delete record
        public async Task<Customer> DeleteCustomer(int? id)
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

                HttpResponseMessage response = await client.DeleteAsync("/api/CustomerWebApi/" + id);
                string stringData = response.Content.
            ReadAsStringAsync().Result;
                Customer data = JsonConvert.DeserializeObject<Customer>(stringData);

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
