using ContactListAPI.Models;

namespace ContactListAPI
{
    //Seeduję do bazy podstawowe dane, aby nigdy nie była pusta
    public class ContactsSeeder
    {
        private readonly ContactListContext _dbContext;

        public ContactsSeeder(ContactListContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect()) {

                //W przypadku pustej tabeli Contacts dodaję podstawowy wpis

                if (!_dbContext.Contacts.Any())
                {
                    var contacts = GetContacts();
                    _dbContext.Contacts.AddRange(contacts);
                    _dbContext.SaveChanges();
                }

                //W przypadku braku danych w tabelach słownikowych, uzupełniam je o podstawowe wpisy

                if (!_dbContext.Categories.Any())
                {
                    var categories = GetCategories();
                    _dbContext.Categories.AddRange(categories);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.SubCategories.Any())
                {
                    var subCategories = GetSubCategories();
                    _dbContext.SubCategories.AddRange(subCategories);
                    _dbContext.SaveChanges();
                }

            }
        }

        private IEnumerable<Contact> GetContacts()
        {

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    Name = "Aureliusz",
                    Lastname = "Czarnacki",
                    Email = "aurelczarnacki@gmail.com",
                    Password = "Standard1!",
                    Category = "Inny",
                    SubCategory = "Ja",
                    Phone = "797420809",
                    BirthDate = DateTime.Parse("2000-03-11")
                }
            };

            return contacts;
        }

        private IEnumerable<Category> GetCategories()
        {
            var categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Prywatny",
                },

                new Category()
                {
                    Name = "Służbowy",
                },

                new Category()
                {
                    Name = "Inny",
                }

            };

            return categories;
        }

        private IEnumerable<SubCategory> GetSubCategories()
        {
            var subCategories = new List<SubCategory>()
            {
                new SubCategory()
                {
                    Name = "Szef",
                },

                new SubCategory()
                {
                    Name = "Klient",
                },

                new SubCategory()
                {
                    Name = "Współpracownik",
                },
            };

            return subCategories;
        }

    }
}
 