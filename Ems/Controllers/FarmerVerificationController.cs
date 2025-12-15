using JiksAgriFarm.Data.Models.Domain;
using JiksAgriFarm.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JiksAgriFarm.Controllers
{
    public class FarmerVerificationController : Controller
    {
        private readonly IFarmerVerificationRepository _verifyRepo;
        private readonly IFarmerRepository _farmerRepo;
        private readonly IWebHostEnvironment _env;

        public FarmerVerificationController(
            IFarmerVerificationRepository verifyRepo,
            IFarmerRepository farmerRepo,
            IWebHostEnvironment env)
        {
            _verifyRepo = verifyRepo;
            _farmerRepo = farmerRepo;
            _env = env;
        }

        // -----------------------------
        // FARMER UPLOAD VERIFICATION PAGE
        // -----------------------------
        public IActionResult Upload(int farmerId)
        {
            ViewBag.FarmerID = farmerId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int farmerId, IFormFile DocumentUpload)
        {
            if (DocumentUpload == null || DocumentUpload.Length == 0)
            {
                TempData["Error"] = "Please upload a valid PDF or image document.";
                return RedirectToAction("Upload", new { farmerId });
            }

            // Save document into folder wwwroot/uploads
            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = $"{Guid.NewGuid()}_{DocumentUpload.FileName}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await DocumentUpload.CopyToAsync(stream);
            }

            var verification = new FarmerVerification
            {
                FarmerID = farmerId,
                DocumentPath = "/uploads/" + fileName,
                UploadedDate = DateTime.Now,
                VerificationStatus = "Pending"
            };

            await _verifyRepo.AddVerificationAsync(verification);

            TempData["Success"] = "Your document was submitted for verification.";
            return RedirectToAction("Status", new { farmerId });
        }

        // --------------------------------
        // FARMER VIEW VERIFICATION STATUS
        // --------------------------------
        public async Task<IActionResult> Status(int farmerId)
        {
            var status = await _verifyRepo.GetByFarmerIdAsync(farmerId);
            return View(status);
        }

        // -------------------------------------
        // ADMIN: VIEW ALL PENDING VERIFICATIONS
        // -------------------------------------
        public async Task<IActionResult> Pending()
        {
            var list = await _verifyRepo.GetAllPendingAsync();
            return View(list);
        }

        // -------------------------------------
        // ADMIN: APPROVE VERIFICATION
        // -------------------------------------
        public async Task<IActionResult> Approve(int id)
        {
            await _verifyRepo.ApproveAsync(id);
            TempData["Success"] = "Farmer verification approved!";
            return RedirectToAction("Pending");
        }

        // -------------------------------------
        // ADMIN: REJECT VERIFICATION
        // -------------------------------------
        public IActionResult Reject(int id)
        {
            ViewBag.VerificationID = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int verificationId, string reason)
        {
            await _verifyRepo.RejectAsync(verificationId, reason);

            TempData["Error"] = "Verification rejected.";
            return RedirectToAction("Pending");
        }
    }
}
