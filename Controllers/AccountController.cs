using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using hrbs_project.Models;
using System.Linq;
using System;
using System.Net;
using System.Net.Mail;

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

        // ================= FORGOT PASSWORD =================

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                TempData["Error"] = "Email not found!";
                return RedirectToAction("Index", "Home");
            }

            // Generate token
            string token = Guid.NewGuid().ToString();

            user.ResetToken = token;
            user.TokenExpiry = DateTime.Now.AddMinutes(30);

            _context.SaveChanges();

            // Create reset link
            string resetLink = Url.Action("ResetPassword", "Account",new { token = token }, Request.Scheme);

            // Send Email
            SendEmail(user.Email, resetLink);

            TempData["Success"] = "Reset link sent to your email!";
            return RedirectToAction("Index", "Home");
        }

        // ================= RESET PASSWORD (GET) =================

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.ResetToken == token);

            if (user == null || user.TokenExpiry < DateTime.Now)
            {
                return Content("Invalid or expired token");
            }

            ViewBag.Token = token;
            return View();
        }

        // ================= RESET PASSWORD (POST) =================

        [HttpPost]
        public IActionResult ResetPassword(string token, string newPassword, string confirmPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.ResetToken == token);

            if (user == null || user.TokenExpiry < DateTime.Now)
            {
                return Content("Invalid or expired token");
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Message = "Passwords do not match!";
                ViewBag.Token = token;
                return View();
            }

            // 👉 UPDATE PASSWORD
            user.Password = newPassword;

            // Clear token
            user.ResetToken = null;
            user.TokenExpiry = null;

            _context.SaveChanges();

            ViewBag.Message = "Password updated successfully!";
            return View();
        }

        // ================= EMAIL FUNCTION =================

        private void SendEmail(string toEmail, string link)
        {
            var fromEmail = "mihirharsoda1@gmail.com";
            var password = "qqxsugmcadxcnpzz";

            var message = new MailMessage();
            message.To.Add(toEmail);
            message.From = new MailAddress(fromEmail);
            message.Subject = "Reset Password";
            message.Body = $"Click here to reset your password: <br/><a href='{link}'>Reset Password</a>";
            message.IsBodyHtml = true;

            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(fromEmail, password);
            smtp.EnableSsl = true;

            smtp.Send(message);
        }
    }
}