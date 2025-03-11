using LibraryManager.Core.Entities;

namespace LibraryManager.Application.Models
{
    public class LoanItemViewModel
    {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int BookId { get; set; }
            public DateTime LoanDate { get; set; }
            public DateTime? ReturnDate { get; set; }
            public DateTime? ActualReturnDate { get; set; }
            public int DelayedDays { get; set; }

            public static LoanItemViewModel FromEntity(Loan loan)
            {
                return new LoanItemViewModel
                {
                    Id = loan.Id,
                    UserId = loan.UserId,
                    BookId = loan.BookId,
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate,
                    ActualReturnDate = loan.ActualReturnDate,
                    DelayedDays = loan.GetDelayedDays()
                };
            }
    }
}
