using hrbs_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hrbs_project.Controllers
{
    public class RatingReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RatingReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ MAIN PAGE
        public async Task<IActionResult> Reviews()
        {
            var ratingsList = await (from rr in _context.RatingReviews
                                     join u in _context.Users on rr.user_id equals u.Id
                                     join r in _context.Rooms on rr.room_id equals r.Id
                                     orderby rr.review_date descending
                                     select new RatingReview
                                     {
                                         id = rr.id,
                                         user_id = rr.user_id,
                                         room_id = rr.room_id,
                                         rating = rr.rating,
                                         review = rr.review,
                                         review_date = rr.review_date,
                                         is_read = rr.is_read,
                                         UserName = u.Name,
                                         RoomName = r.Name
                                     }).ToListAsync();

            var model = new RatingsReviewsViewModel
            {
                RatingsReviews = ratingsList,
                TotalCount = ratingsList.Count,
                AverageRating = ratingsList.Any() ? Math.Round(ratingsList.Average(x => x.rating), 1) : 0,
                UnreadCount = ratingsList.Count(x => !x.is_read)
            };

            return View("~/Views/Admin/Reviews.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> MarkAsRead(int id)
        {
            var data = await _context.RatingReviews.FindAsync(id);
            if (data != null)
            {
                data.is_read = true;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(int id)
        {
            var data = await _context.RatingReviews.FindAsync(id);
            if (data != null)
            {
                _context.RatingReviews.Remove(data);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> MarkAllAsRead()
        {
            var list = await _context.RatingReviews.Where(x => !x.is_read).ToListAsync();

            foreach (var item in list)
                item.is_read = true;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteSelected([FromBody] int[] ids)
        {
            var list = await _context.RatingReviews.Where(x => ids.Contains(x.id)).ToListAsync();

            _context.RatingReviews.RemoveRange(list);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}