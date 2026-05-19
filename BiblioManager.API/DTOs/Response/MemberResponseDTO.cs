namespace BiblioManager.API.DTOs.Response
{
    public class MemberResponseDTO
    {
        public int Id { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
