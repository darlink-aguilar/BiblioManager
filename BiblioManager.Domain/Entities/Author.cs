namespace BiblioManager.Domain.Entities

{
    public class Author : AuditBase
    {
        public string FirstName { get; set; } = string.Empty; // Campo obligatorio
        public string LastName { get; set; } = string.Empty; // Campo obligatorio
        public string Nationality { get; set; } = string.Empty; // Campo obligatorio
        public DateTime? BirthDate { get; set; } // No es obligatorio

        // Navigation Property
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
