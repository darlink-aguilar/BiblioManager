namespace BiblioManager.API.DTOs.Response
{
    public class BookResponseDTO
    {
        public int Id { get; set; }
        public string Isbn { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty; 
        public string Synopsis { get; set; } = string.Empty; 
        public DateTime? PublicationDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
