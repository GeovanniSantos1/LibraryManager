namespace LibraryManager.Application.Models
{
    public class BookReviewViewModel
    {
        public string Id { get; set; }
        public string BookId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 