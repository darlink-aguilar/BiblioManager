namespace BiblioManager.Domain.Entities

{
    public class Category : AuditBase
    {
        public string Name { get; set; } = string.Empty; // Campo obligatorio
        public string Description { get; set; } = string.Empty; // Campo obligatorio

        // Navigation Property

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
