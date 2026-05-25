namespace BiblioManager.API.DTOs.Request
{
    public class BookRequestDTO
    {
        public string Isbn { get; set; } = string.Empty; 
        public string Title { get; set; } = string.Empty; 
        public string Synopsis { get; set; } = string.Empty; 
        public DateTime? PublicationDate { get; set; }
        public int CategoryId { get; set; }

    }
}
