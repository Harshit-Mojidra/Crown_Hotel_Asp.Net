using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using hrbs_project.Models;

namespace hrbs_project.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= SESSION =================
        private bool IsAdminLoggedIn()
        {
            return HttpContext.Session.GetString("Admin") != null;
        }

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Simple hardcoded admin (you can change later to DB)
            if (username == "admin" && password == "123")
            {
                HttpContext.Session.SetString("Admin", "true");
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // ================= LOGOUT =================

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Login");
        }

        // ================= DASHBOARD =================

        public IActionResult Dashboard()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            ViewBag.NewUsers = _context.Users.Count();
            ViewBag.ActiveUsers = _context.Users.Count(u => u.Status == "active");
            ViewBag.InactiveUsers = _context.Users.Count(u => u.Status == "inactive");

            ViewBag.TotalBookings = _context.BookingOrder.Count();

            return View();
        }

        // ================= USERS =================

        public IActionResult Users(string search)
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u =>
                    u.Name.Contains(search) ||
                    u.Email.Contains(search));
            }

            var users = query.ToList();

            return View(users);
        }

        // ================= TOGGLE USER STATUS =================

        [HttpPost]
        public IActionResult ToggleUserStatus(int id)
        {
            if (!IsAdminLoggedIn())
                return Json(new { success = false, message = "Unauthorized" });

            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return Json(new { success = false, message = "User not found" });

            user.Status = (user.Status == "active") ? "inactive" : "active";

            _context.SaveChanges();

            return Json(new { success = true, status = user.Status });
        }
    }
}