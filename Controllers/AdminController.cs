using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using hrbs_project.Models;
using System;
using System.IO;

// CONTROLLER FOR ADMIN DASHBOARD
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
            if (username == "admin" && password == "123")
            {
                HttpContext.Session.SetString("Admin", "true");
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

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

            var bookings = _context.BookingOrder.ToList();

            // COUNTS
            ViewBag.TotalBookings = bookings.Count;
            ViewBag.PaidBookings = bookings.Count(b => b.status == "Paid");
            ViewBag.CancelledBookings = bookings.Count(b => b.status == "Cancelled");

            // AMOUNTS
            ViewBag.TotalAmount = bookings.Sum(b => (decimal?)b.paid_amount) ?? 0;
            ViewBag.PaidAmount = bookings
                .Where(b => b.status == "Paid")
                .Sum(b => (decimal?)b.paid_amount) ?? 0;

            ViewBag.CancelledAmount = bookings
                .Where(b => b.status == "Cancelled")
                .Sum(b => (decimal?)b.paid_amount) ?? 0;

            // USERS
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.ActiveUsers = _context.Users.Count(u => u.Status == "active");
            ViewBag.InactiveUsers = _context.Users.Count(u => u.Status == "inactive");
            ViewBag.UnverifiedUsers = _context.Users.Count(u => u.Status == "unverified");

            return View();
        }

        // ================= BOOKINGS =================
        // SHOW ALL PAID BOOKINGS
        public IActionResult NewBooking()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var bookings = _context.BookingOrder
                .Where(b => b.status == "Paid")
                .OrderByDescending(b => b.booking_date)
                .ToList();

            return View(bookings);
        }

        // ALL BOOKINGS (PAID + CANCELLED)
        public IActionResult BookingRecords()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var bookings = _context.BookingOrder
                .OrderByDescending(b => b.booking_date)
                .ToList();

            return View(bookings);
        }

        // CANCELLED BOOKINGS
        public IActionResult RefundBooking()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var bookings = _context.BookingOrder
                .Where(b => b.status == "Cancelled")
                .ToList();

            return View(bookings);
        }

        // CANCEL BOOKING
        [HttpPost]
        public IActionResult CancelBooking(int id)
        {
            var booking = _context.BookingOrder
                .FirstOrDefault(b => b.Id == id);

            if (booking != null)
            {
                booking.status = "Cancelled";

                // Increase room availability
                var room = _context.Rooms.FirstOrDefault(r => r.Id == booking.room_id);
                if (room != null)
                {
                    room.AvailableRooms += 1;
                }

                _context.SaveChanges();
            }

            return RedirectToAction("RefundBooking");
        }

        // ================= ROOMS =================
        public IActionResult Rooms()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var rooms = _context.Rooms.ToList();
            return View(rooms);
        }

        public IActionResult RoomDetails(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);

            if (room == null)
                return NotFound();

            var bookings = _context.BookingOrder
                .Where(b => b.room_id == id && b.status == "Paid")
                .Select(b => new { b.check_in, b.check_out })
                .ToList();

            ViewBag.BookedDates = bookings;

            return View(room);
        }

        // ================= SETTINGS =================
        public IActionResult Settings()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var settings = _context.Settings.FirstOrDefault();

            if (settings == null)
            {
                settings = new Settings
                {
                    SiteTitle = "Crown Hotel",
                    About = "Default About",
                    IsShutdown = false
                };
            }

            return View(settings);
        }

        [HttpPost]
        public IActionResult UpdateGeneralSettings(Settings model)
        {
            var data = _context.Settings.FirstOrDefault();

            if (data != null)
            {
                data.SiteTitle = model.SiteTitle;
                data.About = model.About;
                _context.SaveChanges();
            }

            return RedirectToAction("Settings");
        }

        [HttpPost]
        public IActionResult UpdateContactSettings(Settings model)
        {
            var data = _context.Settings.FirstOrDefault();

            if (data != null)
            {
                data.Address = model.Address;
                data.Phone1 = model.Phone1;
                data.Email = model.Email;
                data.Facebook = model.Facebook;
                data.Iframe = model.Iframe;

                _context.SaveChanges();
            }

            return RedirectToAction("Settings");
        }

        // ================= TEAM =================
        [HttpPost]
        public IActionResult AddTeamMember(string Name, IFormFile ImageFile)
        {
            if (ImageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/team/", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                TeamMember member = new TeamMember
                {
                    Name = Name,
                    ImagePath = "/images/team/" + fileName
                };

                _context.TeamMembers.Add(member);
                _context.SaveChanges();
            }

            return RedirectToAction("Settings");
        }

        [HttpPost]
        public IActionResult DeleteTeamMember(int id)
        {
            var member = _context.TeamMembers.Find(id);

            if (member != null)
            {
                _context.TeamMembers.Remove(member);
                _context.SaveChanges();
            }

            return RedirectToAction("Settings");
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
                    (u.Name != null && u.Name.Contains(search)) ||
                    (u.Email != null && u.Email.Contains(search))
                );
            }

            return View(query.ToList());
        }

        [HttpPost]
        public IActionResult ToggleUserStatus(int id)
        {
            if (!IsAdminLoggedIn())
                return Json(new { success = false });

            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return Json(new { success = false });

            user.Status = (user.Status == "active") ? "inactive" : "active";
            _context.SaveChanges();

            return Json(new { success = true, status = user.Status });
        }
    }
}