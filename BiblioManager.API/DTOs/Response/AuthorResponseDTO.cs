namespace BiblioManager.API.DTOs.Response
{
    public class AuthorResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty; 
        public string LastName { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
