namespace BiblioManager.Domain.Entities

{
    public class Member : AuditBase
    {
        public string Dni { get; set; } = string.Empty; // Campo obligatorio
        public string FullName { get; set; } = string.Empty; // Campo obligatorio
        public string Email { get; set; } = string.Empty; // Campo obligatorio
        public DateTime? BirthDate { get; set; } // No es obligatorio
        public bool IsActive { get; set; } = true; // Campo obligatorio, valor por defecto true

        // Navigation Property
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
