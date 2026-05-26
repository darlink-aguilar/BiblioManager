using BiblioManager.Domain.Enums;

namespace BiblioManager.API.DTOs.Request
{
    public class UpdateLoanStatusDTO
    {
        public LoanStatus Status { get; set; }
    }
}