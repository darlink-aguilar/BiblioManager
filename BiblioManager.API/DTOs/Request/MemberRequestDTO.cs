namespace BiblioManager.API.DTOs.Request
{
    public class MemberRequestDTO
    {
        public string Dni { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; 
        public DateTime? BirthDate { get; set; } 
    }
}
