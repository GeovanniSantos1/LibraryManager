namespace LibraryManager.Application.Models
{
    public class LoanStatusViewModel
    {
        public string Status { get; set; }
        public int? DelayedDays { get; set; }
        public int? RemainingDays { get; set; }
        public bool IsReturned { get; set; }
    }
}
