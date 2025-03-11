namespace LibraryManager.Core.Entities
{
    public class Loan : BaseEntity
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }

        public Loan() : base()
        {
        }

        public Loan(int userId, int bookId) : base()
        {
            UserId = userId;
            BookId = bookId;
            LoanDate = DateTime.Now;
            ReturnDate = DateTime.Now.AddDays(14); // Prazo padrão de 14 dias para devolução
        }

        public int GetDelayedDays()
        {
            if (!ActualReturnDate.HasValue && DateTime.Now > ReturnDate)
            {
                return (int)(DateTime.Now - ReturnDate.Value).TotalDays;
            }
            else if (ActualReturnDate.HasValue && ActualReturnDate > ReturnDate)
            {
                return (int)(ActualReturnDate.Value - ReturnDate.Value).TotalDays;
            }
            return 0;
        }
    }
}