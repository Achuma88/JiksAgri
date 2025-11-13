using Ems.Data.Models.Domain;
using Ems.Data.Repositories;
using Ems.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using System.Threading.Tasks;

namespace Ems.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public LoginController(ILoginRepository loginRepository, IEmployeeRepository employeeRepository)
        {
            _loginRepository = loginRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Login login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var isValidUser = await _loginRepository.ValidateUserAsync(login.EmpEmail, login.EmpPassword);

            if (!isValidUser)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(login);
            }

            var employee = await _employeeRepository.GetEmployeeByEmailAsync(login.EmpEmail);

            if (employee == null || employee.EmpStatus != "Active")
            {
                TempData["LoginError"] = "Your account is not active or does not exist.";
                return View(login);
            }

            // ✅ Create user claims
            var claims = new List<Claim>
            {
              new Claim(ClaimTypes.NameIdentifier, employee.EmpID.ToString()),
              new Claim(ClaimTypes.Name, employee.EmpName),
              new Claim(ClaimTypes.Email, employee.EmpEmail),
              new Claim(ClaimTypes.Role, employee.EmpRole)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // "Remember me" logic here if needed
                ExpiresUtc = DateTime.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

            if (employee.EmpStatus == "Active")
            {
                if (employee.EmpRole == "Admin")
                {
                   
                    return RedirectToAction("Index", "Home");
                }
                else if (employee.EmpRole == "Staff")
                {
                   
                    return RedirectToAction("Index", "Staff");
                }
                else if (employee.EmpRole == "Intern")
                {
                    
                    return RedirectToAction("Index", "Staff");
                }
                else 
                {
                   
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
               
                return RedirectToAction("Login", "Account");
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Index", "Login");
        }


    }
}
