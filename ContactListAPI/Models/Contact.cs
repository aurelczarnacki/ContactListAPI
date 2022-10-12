using System.ComponentModel.DataAnnotations;

namespace ContactListAPI.Models
{
    //Model tabeli z wszystkimi informacjami kontaktów
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Category { get; set; }
        public string? SubCategory { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
