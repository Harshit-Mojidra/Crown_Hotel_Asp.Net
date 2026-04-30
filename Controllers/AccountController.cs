using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using hrbs_project.Models;
using System.Linq;

namespace hrbs_project.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= LOGIN PAGE =================

        [HttpGet]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }

        // ================= LOGIN SUBMIT =================

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("User", user.Email ?? "");

                if (TempData["ReturnUrl"] != null)
                {
                    return Redirect(TempData["ReturnUrl"].ToString());
                }

                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Invalid login!";
            return RedirectToAction("Index", "Home");
        }

        // ================= LOGOUT =================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ================= REGISTER =================

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                TempData["Error"] = "Passwords do not match!";
                return RedirectToAction("Index", "Home");
            }

            string fileName = "";

            // ✅ IMAGE UPLOAD
            if (model.ImageFile != null)
            {
                fileName = Path.GetFileName(model.ImageFile.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    model.ImageFile.CopyTo(stream);
                }

                model.ImagePath = fileName;
            }

            // ✅ SAVE USER
            var newUser = new User()
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Phone = model.Phone,
                Address = model.Address,
                Pincode = model.Pincode,
                DOB = model.DOB,
                ImagePath = model.ImagePath
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            TempData["Success"] = "Registered successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}