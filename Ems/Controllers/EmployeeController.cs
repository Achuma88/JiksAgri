using Ems.Data.Models.Domain;
using Ems.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ems.UI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<IActionResult> Add()
        {
            ViewBag.Roles = new List<string> { "Admin", "Staff", "Intern" };
            ViewBag.Statuses = new List<string> { "Active", "Inactive" };
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Add(Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(employee);
                bool addEmp = await _employeeRepository.AddAsync(employee);
                if (addEmp)
                {
                    TempData["SuccessMessage"] = "Sucessfully Added";
                }
                else
                {
                    TempData["ErrorMessage"] = "Could not add";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Hebana!! Something went wrong!!!";
            }
            return RedirectToAction(nameof(DisplayAll));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            // Map Employee to EditEmployee
            var editEmployee = new EditEmployee
            {
                EmpID = employee.EmpID,
                EmpRole = employee.EmpRole,
                EmpStatus = employee.EmpStatus
                // Do NOT map password fields here if they are not required in the edit model
            };

            // Populate ViewBag if needed for dropdowns
            ViewBag.Roles = new List<string> { "Admin", "Staff","Intern" };
            ViewBag.Statuses = new List<string> { "Active", "Inactive" };

            return View(editEmployee);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditEmployee editEmployee)
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
                ViewBag.Roles = new List<string> { "Admin", "Staff","Intern" };
                ViewBag.Statuses = new List<string> { "Active", "Inactive" };
                return View(editEmployee);
            }

          var editResult= await _employeeRepository.UpdateEmployeeAsync(editEmployee);
            if (editResult)
            {
                TempData["SuccessMessage"] = "Employee Updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error from Updating employee.";
            }
            return RedirectToAction("DisplayAll");
        }



        public async Task<IActionResult> DisplayAll(string? searchTerm)
        {
            IEnumerable<Employee> employees;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                employees = await _employeeRepository.GetAllAsync();
            }
            else
            {
                // Search by name, email, role, status
                employees = await _employeeRepository.SearchEmployeesAsync(searchTerm);
            }

            return View(employees);
        }


       
        public async Task<IActionResult> Delete(int EmpID)
        {
            var deleteResult = await _employeeRepository.DeleteAsync(EmpID);
            if (deleteResult)
            {
                TempData["SuccessMessage"] = "Employee deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error deleting employee.";
            }

            return RedirectToAction(nameof(DisplayAll));
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateStaff()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int empId))
            {
                return RedirectToAction("Index", "Login");
            }

            var employee = await _employeeRepository.GetByIdAsync(empId);
            if (employee == null)
                return NotFound();

            var updateModel = new Staff
            {
                EmpID = employee.EmpID,
                EmpName = employee.EmpName,
                EmpSurname = employee.EmpSurname,
                EmpEmail = employee.EmpEmail,
                EmpPhone = employee.EmpPhone
            };

            // Pass TempData messages to ViewBag to show modal after redirect
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(updateModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStaff(Staff staff)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int empId))
            {
                TempData["ErrorMessage"] = "Session expired.";
                return RedirectToAction("Index", "Login");
            }

            staff.EmpID = empId;

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors.";
                return View(staff);
            }

            var result = await _employeeRepository.UpdateInfoAsync(staff);
            if (!result)
            {
                TempData["ErrorMessage"] = "Update failed.";
                return View(staff);
            }

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile", "Employee"); // redirect to Profile page
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateAdmin()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int empId))
            {
                return RedirectToAction("Index", "Login");
            }

            var employee = await _employeeRepository.GetByIdAsync(empId);
            if (employee == null)
                return NotFound();

            var updateModel = new Admin
            {
                EmpID = employee.EmpID,
                EmpName = employee.EmpName,
                EmpSurname = employee.EmpSurname,
                EmpEmail = employee.EmpEmail,
                EmpPhone = employee.EmpPhone
            };

            // Pass TempData messages to ViewBag to show modal after redirect
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(updateModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAdmin(Admin admin)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int empId))
            {
                TempData["ErrorMessage"] = "Session expired.";
                return RedirectToAction("Index", "Login");
            }

            admin.EmpID = empId;

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors.";
                return View(admin);
            }

            var result = await _employeeRepository.UpdateAdminAsync(admin);
            if (!result)
            {
                TempData["ErrorMessage"] = "Update failed.";
                return View(admin);
            }

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile", "Employee"); // redirect to Profile page
        }






        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ResetPassword()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure the claim is present and valid
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int empId))
            {
                // Invalid session or user ID
                return RedirectToAction("Index", "Staff");
            }

            var employee = await _employeeRepository.GetByIdAsync(empId);
            if (employee == null)
                return NotFound();

            var updateModel = new Admin
            {
                EmpID = employee.EmpID
            };

            return View(updateModel);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPassword reset)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int empId))
            {
                return RedirectToAction("Index", "Login");
            }

            reset.EmpID = empId;

            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            var result = await _employeeRepository.UpdatePasswordAsync(reset);
            if (!result)
            {
                TempData["ErrorMessage"] = "Something went wrong while updating the password.";
                return View(reset);
            }

           
            TempData["SuccessMessage"] = "Password updated successfully!";
            return View(reset);

        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AdminResetPassword()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure the claim is present and valid
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int empId))
            {
                // Invalid session or user ID
                return RedirectToAction("Index", "Login");
            }

            var employee = await _employeeRepository.GetByIdAsync(empId);
            if (employee == null)
                return NotFound();

            var resetModel = new ResetPassword
            {
                EmpID = employee.EmpID
            };

            return View(resetModel);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminResetPassword(ResetPassword reset)
        {
            // Retrieve EmpID again from the claims to avoid tampering
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int empId))
            {
                return RedirectToAction("Index", "Login");
            }

            reset.EmpID = empId; // Overwrite with trusted ID from claims

            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            var result = await _employeeRepository.UpdatePasswordAsync(reset);
            if (!result)
            {
                TempData["ErrorMessage"] = "Something went wrong while updating the password.";
                return View(reset);
            }

            TempData["SuccessMessage"] = "Password updated successfully!";
            return RedirectToAction("Profile", "Employee"); // or your desired redirect
        }




        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int empId))
            {
                return RedirectToAction("Index", "Login");
            }

            var employee = await _employeeRepository.GetByIdAsync(empId);
            if (employee == null)
                return NotFound();

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(employee);
        }

        [Authorize]
        public async Task<IActionResult> AdminProfile()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier); // EmpID
            var role = User.FindFirstValue(ClaimTypes.Role); // Optional

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int empId))
            {
                // Redirect to login or error if EmpID is missing or invalid
                return RedirectToAction("Index", "Login");
            }

            var employee = await _employeeRepository.GetAdminProfileAsync(empId);

            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(employee);
        }
        public async Task<IActionResult> GetActive()
        {
            var result = await _employeeRepository.GetActiveEmployeesAsync();
            return View(result); // Pass the data to the view
        }
       

        

    }
}

