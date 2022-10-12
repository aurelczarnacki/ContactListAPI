using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using ContactListAPI.Models;

namespace ContactListAPI.Models
{


    public class ContactListContext : DbContext
    {

        public ContactListContext(DbContextOptions<ContactListContext> options) : base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }


    }



}
