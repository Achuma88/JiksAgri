using Ems.Data.Models.Domain;
using Ems.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Ems.UI.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffRepository _staffRepository;

        public StaffController(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }
        public IActionResult Index()
        {
            return View();
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateAsync(Staff staff)
        //{
        //    // Remove ModelState for EmpID so binding uses the posted value
        //    ModelState.Remove(nameof(staff.EmpID));

        //    Console.WriteLine("EmpID from form (after removing ModelState): " + staff.EmpID);

        //    if (!ModelState.IsValid)
        //    {
        //        return View(staff);
        //    }

        //    await _staffRepository.UpdateInfoAsync(staff);
        //    // ...
        //}

        [HttpGet]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var employee = await _staffRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            // Map Employee to EditEmployee
            var emp = new Employee
            {
                EmpID = employee.EmpID,

                // Do NOT map password fields here if they are not required in the edit model
            };

            return View(emp);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Any())
                    {
                        Console.WriteLine($"Key: {entry.Key}, Errors: {string.Join(", ", entry.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }

                return View(employee);
            }

            await _staffRepository.UpdatePasswordAsync(employee);
            TempData["msg"] = "Employee updated successfully!";
            return RedirectToAction("DisplayAll");
        }
        public async Task<IActionResult> Profile(int EmpID)
        {
            var profileResult = await _staffRepository.GetProfileAsync(EmpID);
            return View(profileResult);
        }
        public IActionResult Intern()
        {
            return View();
        }
    }
}
