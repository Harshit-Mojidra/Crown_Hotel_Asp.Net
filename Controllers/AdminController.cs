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

        // ================= SESSION HELPER =================
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

            // USER COUNTS
            ViewBag.NewUsers = _context.Users.Count();
            ViewBag.ActiveUsers = _context.Users.Count(u => u.Status == "active");
            ViewBag.InactiveUsers = _context.Users.Count(u => u.Status == "inactive");

            // BOOKING COUNTS
            ViewBag.TotalBookings = _context.BookingOrder.Count();
            ViewBag.PendingBookings = _context.BookingOrder.Count(b => b.status == "pending");
            ViewBag.ConfirmedBookings = _context.BookingOrder.Count(b => b.status == "confirmed");
            ViewBag.CancelledBookings = _context.BookingOrder.Count(b => b.status == "cancelled");

            return View();
        }

        // ================= BOOKINGS =================
        [HttpGet]
        public IActionResult NewBookings()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RefundBookings()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FeaturesFacilities()
        {
            return View();
        }
        // NEW BOOKINGS (Pending)
        public IActionResult NewBooking()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var newBookings = _context.BookingOrder
                .Where(b => b.status == "pending")
                .ToList();

            return View(newBookings);
        }

        // BOOKING RECORDS (Confirmed + Cancelled)
        public IActionResult BookingRecords()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var bookings = _context.BookingOrder
                .Where(b => b.status == "confirmed" || b.status == "cancelled")
                .ToList();

            return View(bookings);
        }

        // REFUND PAGE (Cancelled)
        public IActionResult RefundBooking()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var refunds = _context.BookingOrder
                .Where(b => b.status == "cancelled")
                .ToList();

            return View(refunds);
        }

        // ================= ACTIONS =================

        // CONFIRM BOOKING
        [HttpPost]
        public IActionResult ConfirmBooking(int id)
        {
            var booking = _context.BookingOrder
                .FirstOrDefault(b => b.user_id == id);

            if (booking != null)
            {
                booking.status = "confirmed";
                _context.SaveChanges();
            }

            return RedirectToAction("BookingRecords");
        }

        // CANCEL BOOKING
        [HttpPost]
        public IActionResult CancelBooking(int id)
        {
            var booking = _context.BookingOrder
                .FirstOrDefault(b => b.user_id == id);

            if (booking != null)
            {
                booking.status = "cancelled";
                _context.SaveChanges();
            }

            return RedirectToAction("RefundBooking");
        }

        // ================= ROOMS =================
        public IActionResult Rooms()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

            var rooms = _context.Rooms.ToList();
            return View(rooms);
        }

        public IActionResult RoomDetails(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);

            if (room == null)
                return NotFound();

            var existingBookings = _context.BookingOrder
                .Where(b => b.room_id == id && b.status == "confirmed")
                .Select(b => new { b.check_in, b.check_out })
                .ToList();

            ViewBag.BookedDates = existingBookings;

            return View(room);
        }

        // ================= SETTINGS =================
        public IActionResult Settings()
        {
            if (!IsAdminLoggedIn()) return RedirectToAction("Login");

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
        [HttpPost]
        public IActionResult AddTeamMember(string Name, IFormFile ImageFile)
        {
            if (ImageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileName);

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
        public IActionResult EditSettings()
        {
            return View();
        }
        public IActionResult AddTeamMember()
        {
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