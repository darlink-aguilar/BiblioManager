namespace BiblioManager.Domain.Entities

{
    public class Book : AuditBase
    {
        public string Isbn { get; set; } = string.Empty; // Campo obligatorio
        public string Title { get; set; } = string.Empty; // Campo obligatorio
        public string Synopsis {get; set; } = string.Empty; // Campo obligatorio
        public DateTime? PublicationDate { get; set; } // No es obligatorio

        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation Property
        public Category Category { get; set; } = null!;
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
