namespace ContactListAPI.Models
{
    //Model dodatkowy klasy kontakt służacy do mapowania wybranych informacji do wyświetlenia użytkownikowi
    public class ContactDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}
