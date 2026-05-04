using hrbs_project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Razorpay.Api;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace hrbs_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly string razorpayKey = "rzp_test_RUuys0IlvCk3ml";
        private readonly string razorpaySecret = "twEXHg6z6Bwenx4ivnXc7BHl";

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // ================= SESSION =================

        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        private int GetUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        // ================= CREATE ORDER =================

        [HttpPost]
        public IActionResult CreateOrder(int roomId, string checkin, string checkout)
        {
            if (!IsUserLoggedIn())
                return Json(new { error = "login_required" });

            try
            {
                DateTime checkInDate = DateTime.ParseExact(checkin, "yyyy-MM-dd", null);
                DateTime checkOutDate = DateTime.ParseExact(checkout, "yyyy-MM-dd", null);

                int days = (checkOutDate - checkInDate).Days;
                if (days <= 0) days = 1;

                var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);

                if (room == null)
                    return Json(new { error = "room_not_found" });

                // Handle nullable Price
                decimal totalAmount = (room.Price ?? 0) * days;

                RazorpayClient client = new RazorpayClient(razorpayKey, razorpaySecret);

                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount",(int) (totalAmount * 100)); // Amount in paise
                options.Add("currency", "INR");
                options.Add("payment_capture", 1);

                Order order = client.Order.Create(options);

                return Json(new
                {
                    id = order["id"].ToString(),
                    amount = totalAmount,
                    days = days,
                    roomId = roomId,
                    checkin = checkin,
                    checkout = checkout
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // ================= HOME =================

        public IActionResult Index()
        {
            var rooms = _context.Rooms
                .Where(r => r.AvailableRooms > 0)
                .ToList();

            return View(rooms);
        }

        // ================= ROOMS =================

        public IActionResult Rooms(string facilities, int? adults, int? children)
        {
            var rooms = _context.Rooms
                .Where(r => r.AvailableRooms > 0)
                .AsQueryable();

            if (!string.IsNullOrEmpty(facilities))
            {
                var facilityList = facilities.Split(',');
                foreach (var facility in facilityList)
                {
                    if (!string.IsNullOrEmpty(facility.Trim()))
                    {
                        rooms = rooms.Where(r => r.Facilities != null && r.Facilities.Contains(facility.Trim()));
                    }
                }
            }

            // Handle nullable Adult and Children
            if (adults.HasValue && adults.Value > 0)
                rooms = rooms.Where(r => r.Adult.HasValue && r.Adult.Value >= adults.Value);

            if (children.HasValue && children.Value > 0)
                rooms = rooms.Where(r => r.Children.HasValue && r.Children.Value >= children.Value);

            return View(rooms.ToList());
        }

        [HttpPost]
        public IActionResult RoomsFilter(string[] facilities, int? adults, int? children)
        {
            var rooms = _context.Rooms
                .Where(r => r.IsAvailable == true)
                .AsQueryable();

            if (facilities != null && facilities.Length > 0)
            {
                rooms = rooms.Where(r => facilities.All(f => r.Facilities != null && r.Facilities.Contains(f)));
            }

            // Handle nullable Adult and Children
            if (adults.HasValue && adults.Value > 0)
                rooms = rooms.Where(r => r.Adult.HasValue && r.Adult.Value >= adults.Value);

            if (children.HasValue && children.Value > 0)
                rooms = rooms.Where(r => r.Children.HasValue && r.Children.Value >= children.Value);

            return View("Rooms", rooms.ToList());
        }
        public IActionResult RoomDetails(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);

            if (room == null)
                return NotFound();
            // Get booked dates for this room
            var existingBookings = _context.BookingOrder
                .Where(b => b.room_id == id && b.status == "Paid")
                .Select(b => new { b.check_in, b.check_out })
                .ToList();

            ViewBag.BookedDates = existingBookings;
            return View(room);
        }

        // ================= STATIC PAGES =================

        public IActionResult Facilities() => View();
        public IActionResult Contact() => View();
        public IActionResult About() => View();

        // ================= CONFIRM BOOKING =================

        public IActionResult ConfirmBooking(int id, string checkin, string checkout)
        {
            if (!IsUserLoggedIn())
            {
                TempData["ReturnUrl"] = Url.Action("ConfirmBooking", "Home", new { id, checkin, checkout });
                return RedirectToAction("Login", "Account");
            }

            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();

            DateTime checkInDate = DateTime.Now;
            DateTime checkOutDate = DateTime.Now.AddDays(1);
            int days = 1;
            decimal totalPrice = room.Price ?? 0; // Handle nullable Price

            if (DateTime.TryParse(checkin, out checkInDate) &&
                DateTime.TryParse(checkout, out checkOutDate))
            {
                days = (checkOutDate - checkInDate).Days;
                if (days <= 0) days = 1;
                totalPrice = (room.Price ?? 0) * days;
            }

            var model = new BookingViewModel
            {
                RoomId = room.Id,
                RoomName = room.Name,
                Image = room.Image ?? "default-room.jpg",
                Price = room.Price ?? 0,
                CheckIn = checkInDate,
                CheckOut = checkOutDate,
                TotalPrice = totalPrice
            };

            ViewBag.RazorpayKey = razorpayKey;
            return View(model);
        }

        // ================= PAYMENT SUCCESS =================

        public async Task<IActionResult> PaymentSuccess(
        string razorpay_payment_id,
        string razorpay_order_id,
        string razorpay_signature,
        int roomId,
        string name,
        string phone,
        string checkin,
        string checkout)
        {
            try
            {
                // ✅ Step 1: Validate Razorpay response
                if (string.IsNullOrEmpty(razorpay_payment_id) ||
                    string.IsNullOrEmpty(razorpay_order_id) ||
                    string.IsNullOrEmpty(razorpay_signature))
                {
                    return Content("Invalid payment information.");
                }

                // ✅ Step 2: Verify signature
                string generated_signature;

                using (var hmac = new System.Security.Cryptography.HMACSHA256(
                    System.Text.Encoding.UTF8.GetBytes(razorpaySecret)))
                {
                    string data = razorpay_order_id + "|" + razorpay_payment_id;
                    var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
                    generated_signature = BitConverter.ToString(hash).Replace("-", "").ToLower();
                }

                if (generated_signature != razorpay_signature)
                {
                    return Content("Payment verification failed.");
                }

                // ✅ Step 3: Parse dates
                if (!DateTime.TryParseExact(checkin, "yyyy-MM-dd", null,
                    System.Globalization.DateTimeStyles.None, out DateTime checkInDate) ||
                    !DateTime.TryParseExact(checkout, "yyyy-MM-dd", null,
                    System.Globalization.DateTimeStyles.None, out DateTime checkOutDate))
                {
                    return Content("Invalid date format.");
                }

                // ✅ Step 4: Get room
                var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
                if (room == null)
                    return Content("Room not found.");

                int? currentUserId = HttpContext.Session.GetInt32("UserId");
                if (currentUserId == null)
                    return RedirectToAction("Login", "Account");

                int days = (checkOutDate - checkInDate).Days;
                if (days <= 0) days = 1;

                decimal totalAmount = (room.Price ?? 0) * days;

                // ✅ Prevent duplicate booking
                var existingBooking = await _context.BookingOrder
                    .FirstOrDefaultAsync(b => b.order_id == razorpay_order_id);

                if (existingBooking != null)
                {
                    ViewBag.PaymentId = razorpay_payment_id;
                    ViewBag.RoomName = room.Name;
                    ViewBag.Amount = totalAmount;
                    return View();
                }

                // ✅ Save booking
                var booking = new Booking
                {
                    user_id = currentUserId.Value,
                    room_id = room.Id,
                    check_in = checkInDate,
                    check_out = checkOutDate,
                    status = "Paid",
                    order_id = razorpay_order_id,
                    payment_id = razorpay_payment_id,
                    user_name = name,
                    phone = phone,
                    room_name = room.Name,
                    price = room.Price ?? 0,
                    paid_amount = totalAmount,
                    booking_date = DateTime.Now,
                    refund_status = "Pending"
                };

                await _context.BookingOrder.AddAsync(booking);

                if (room.AvailableRooms > 0)
                    room.AvailableRooms -= 1;

                await _context.SaveChangesAsync();

                ViewBag.PaymentId = razorpay_payment_id;
                ViewBag.RoomName = room.Name;
                ViewBag.Amount = totalAmount;
                ViewBag.Name = name;
                ViewBag.CheckIn = checkInDate.ToString("dd MMM yyyy");
                ViewBag.CheckOut = checkOutDate.ToString("dd MMM yyyy");
                ViewBag.Days = days;

                return View();
            }
            catch (Exception ex)
            {
                return Content($"ERROR: {ex.Message}");
            }
        }

        // ================= USER BOOKINGS =================

        public IActionResult MyBookings()
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Account");

            int userId = GetUserId();

            var bookings = _context.BookingOrder
                .Where(b => b.user_id == userId)
                .OrderByDescending(b => b.booking_date)
                .ToList();

            return View(bookings);
        }

        [HttpPost]
        public IActionResult CancelBooking(int id)
        {
            if (!IsUserLoggedIn())
                return Json(new { success = false, message = "Login required" });

            var booking = _context.BookingOrder.Find(id);

            if (booking != null && booking.user_id == GetUserId())
            {
                if (booking.status == "Paid")
                {
                    booking.status = "Cancelled";
                    booking.refund_status = "Processing";

                    // MAKE ROOM AVAILABLE AGAIN
                    var room = _context.Rooms.FirstOrDefault(r => r.Id == booking.room_id);
                    if (room != null)
                    {
                        room.AvailableRooms += 1;
                    }

                    _context.SaveChanges();
                    return Json(new { success = true, message = "Booking cancelled successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "Booking cannot be cancelled" });
                }
            }

            return Json(new { success = false, message = "Booking not found" });
        }

        // GET version for redirects
        public IActionResult CancelBookingGet(int id)
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Account");

            var booking = _context.BookingOrder.Find(id);

            if (booking != null && booking.user_id == GetUserId())
            {
                booking.status = "Cancelled";

                // MAKE ROOM AVAILABLE AGAIN
                var room = _context.Rooms.FirstOrDefault(r => r.Id == booking.room_id);
                if (room != null)
                {
                    room.IsAvailable = true;
                }

                _context.SaveChanges();
                TempData["Message"] = "Booking cancelled successfully";
            }

            return RedirectToAction("MyBookings");
        }

        //public IActionResult RoomDetails(int id)
        //{
        //    var room = _context.Rooms.FirstOrDefault(r => r.Id == id);

        //    if (room == null)
        //        return NotFound();

        //    // Get booked dates for this room
        //    var existingBookings = _context.BookingOrder
        //        .Where(b => b.room_id == id && b.status == "Paid")
        //        .Select(b => new { b.check_in, b.check_out })
        //        .ToList();

        //    ViewBag.BookedDates = existingBookings;

        //    return View(room);
        //}

        // ================= SEARCH AVAILABLE ROOMS =================

        [HttpPost]
        public IActionResult SearchAvailableRooms(DateTime checkin, DateTime checkout, int? adults, int? children)
        {
            // Get all rooms that are available
            var availableRooms = _context.Rooms
                .Where(r => r.IsAvailable == true)
                .AsQueryable();

            // Filter by capacity - handle nullable
            if (adults.HasValue && adults.Value > 0)
                availableRooms = availableRooms.Where(r => r.Adult.HasValue && r.Adult.Value >= adults.Value);

            if (children.HasValue && children.Value > 0)
                availableRooms = availableRooms.Where(r => r.Children.HasValue && r.Children.Value >= children.Value);

            // Exclude rooms that are booked for the selected dates
            var bookedRoomIds = _context.BookingOrder
                .Where(b => b.status == "Paid" &&
                       ((checkin >= b.check_in && checkin < b.check_out) ||
                        (checkout > b.check_in && checkout <= b.check_out) ||
                        (checkin <= b.check_in && checkout >= b.check_out)))
                .Select(b => b.room_id)
                .Distinct()
                .ToList();

            availableRooms = availableRooms.Where(r => !bookedRoomIds.Contains(r.Id));

            var rooms = availableRooms.ToList();

            ViewBag.CheckIn = checkin;
            ViewBag.CheckOut = checkout;
            ViewBag.Adults = adults;
            ViewBag.Children = children;

            return View("Rooms", rooms);
        }
    }
}