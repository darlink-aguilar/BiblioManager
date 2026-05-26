using BiblioManager.Domain.Enums;

namespace BiblioManager.API.DTOs.Response
{
    public class LoanResponseDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; } // Campo obligatorio
        public string MemberName { get; set; } = string.Empty;
        public int BookId { get; set; } // Campo obligatorio
        public string BookName { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; } // Campo obligatorio
        public DateTime DueDate { get; set; } // Campo obligatorio
        public DateTime? ReturnDate { get; set; }
        public LoanStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
