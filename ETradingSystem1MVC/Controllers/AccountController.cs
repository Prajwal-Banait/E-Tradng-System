
using ETradingSystem1.WebApi.Authentication;
using ETradingSystem1MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ETradingSystem1.Mvc.Controllers
{
    public class AccountController : Controller
    {

        //public AccountController()
        //{

        //}
        public IActionResult Login()
        {
            return View();
        }

        // POST: CarTypeMVC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] LoginModel loginmodel)
        {
            var returnloginmodel = await AddLoginModel(loginmodel);

            var handler = new JwtSecurityTokenHandler();

            var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            var token = handler.ReadJwtToken(token2);
            var role = token.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault();

            //if ((role.Value == "Admin") ||
            //   (returnedLoginModel.UserName == "admin") && (returnedLoginModel.Password == "Admin@123") ||
            //   (returnedLoginModel.UserName == "sir") && (returnedLoginModel.Password == "Sir@123"))

                if (role.Value == "Admin")
            {

                return RedirectToAction("HomeAdmin", "Home");
            }
            else if (role.Value == "Customer")
            {
                return RedirectToAction("HomeCustomer", "Home");
            }
            else if (role.Value == "BusinessOwner")
            {
                return RedirectToAction("HomeBusinessOwner", "Home");
            }
            else { 
                return View(returnloginmodel);
            }
            
        }


        public async Task<LoginModel> AddLoginModel(LoginModel loginmodel)
        {
            string baseUrl = "https://localhost:7056";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string stringData = JsonConvert.SerializeObject(loginmodel);
            var contentData = new StringContent(stringData,
        System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/Authenticate/login", contentData);

            if (response.IsSuccessStatusCode)
            {
                string stringJWT = response.Content.
                ReadAsStringAsync().Result;
                JWT jwt = JsonConvert.DeserializeObject
                <JWT>(stringJWT);

                //HttpContext.Session.SetString("token", jwt.Token);
                // TempData["token"] = jwt.Token;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "token", jwt.Token);
            }

             if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            return loginmodel;

        }

        public async Task<IActionResult> Register()
        {
            List<IdentityRole> roles = await GetRoles();
           //var rolesWithoutAdmin = roles.Where(r => r.Name !="Admin");
             var sortedRoles = roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(),Text = rr.Name}).ToList();
            //var sortedRoles = rolesWithoutAdmin.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewData["Role"] = sortedRoles;
            return View();
        }

        // POST: CarTypeMVC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Email,Password,Role")] RegisterModel registermodel)
        {

           var returnedregistermodel= await AddRegisterModel(registermodel);
            if (returnedregistermodel!=null)
            {
                return RedirectToAction("HomeCommon","Home");
            }
            //try
            //{
            //    await AddRegisterModel(registermodel);
            //}
            //catch (Exception)
            //{
            //    if (RegisterModelExists(registermodel.Email) == null)
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //    return RedirectToAction("Account","Login");
            //}

            return View(registermodel);
        }

        private object RegisterModelExists(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<RegisterModel> AddRegisterModel(RegisterModel registermodel)
        {
           
            string baseUrl = "https://localhost:7056";
            HttpClient client = new HttpClient();
           // registermodel.Role = UserRoles.ADMIN;
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string stringData = JsonConvert.SerializeObject(registermodel);
            var contentData = new StringContent(stringData,
        System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/Authenticate/registeradmin", contentData);

            if (response.IsSuccessStatusCode)
            {
                return registermodel;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            return registermodel;
        }


        public IActionResult Logout()
        { 
                 HttpContext.Session.Clear();
                 return RedirectToAction("HomeCommon", "Home");
            
        }

        public async Task<List<IdentityRole>> GetRoles()
        {
            string baseUrl = "https://localhost:7056";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        
            HttpResponseMessage response = await client.GetAsync("/api/Authenticate");
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<IdentityRole> data = JsonConvert.DeserializeObject<List<IdentityRole>>(stringData);

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
    }
}
