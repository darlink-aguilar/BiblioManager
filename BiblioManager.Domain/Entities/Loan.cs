using BiblioManager.Domain.Enums;

namespace BiblioManager.Domain.Entities

{
    public class Loan : AuditBase
    {
        public int MemberId { get; set; } // Campo obligatorio
        public int BookId { get; set; } // Campo obligatorio
        public DateTime LoanDate { get; set; } // Campo obligatorio
        public DateTime DueDate { get; set; } // Campo obligatorio
        public DateTime? ReturnDate { get; set; }
        public LoanStatus Status { get; set; }

        // Navigation Property
        public Book Book { get; set; } = null!;
        public Member Member { get; set; } = null!;

    }
}
