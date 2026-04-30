using System.Collections.Generic;

namespace hrbs_project.Models
{
    public class RatingsReviewsViewModel
    {
        public List<RatingReview> RatingsReviews { get; set; } = new List<RatingReview>();

        public int TotalCount { get; set; }

        public double AverageRating { get; set; }

        public int UnreadCount { get; set; }
    }
}