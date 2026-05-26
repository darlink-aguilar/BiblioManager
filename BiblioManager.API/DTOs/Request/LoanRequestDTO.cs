using BiblioManager.Domain.Enums;

namespace BiblioManager.API.DTOs.Request
{
    public class LoanRequestDTO
    {
        public int MemberId { get; set; } // Campo obligatorio
        public int BookId { get; set; } // Campo obligatorio
        public DateTime DueDate { get; set; } // Campo obligatorio
    }
}
