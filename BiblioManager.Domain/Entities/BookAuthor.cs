namespace BiblioManager.Domain.Entities

{
    public class BookAuthor : AuditBase
    {
        public int BookId { get; set; } // Campo obligatorio
        public int AuthorId { get; set; } // Campo obligatorio

        // Navigation Property
        public Book Book { get; set; } = null!;
        public Author Author { get; set; } = null!; 

    }
}
