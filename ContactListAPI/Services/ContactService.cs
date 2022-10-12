using AutoMapper;
using ContactListAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactListAPI.Services
{
    public interface IContactService
    {
        int Create(Contact contact);
        IEnumerable<ContactDto> GetAll();
        Contact GetById(int id);
        bool Update(int id, Contact c);
        bool Delete(int id);
        string GenerateJwt(LoginDto dto);
    }

    public class ContactService : IContactService
    {
        private readonly ContactListContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Contact> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public ContactService(ContactListContext dbContext, IMapper mapper, IPasswordHasher<Contact> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public Contact GetById(int id)
        {
            var contact = _dbContext.Contacts.FirstOrDefault(x => x.Id == id);


            if (contact == null) return null;

            return contact;
        }


        // Zmapowanie kontaktów do obiektu DTO ma za zadanie przesłanie do klienta tylko wybranych informacji o kontaktach
        public IEnumerable<ContactDto> GetAll()
        {
            var contacts = _dbContext.Contacts.ToList();

            var contactsDtos = _mapper.Map<List<ContactDto>>(contacts);

            return contactsDtos;
        }

        public int Create(Contact contact)
        {
            var newContact = new Contact()
            {
                Name = contact.Name,
                Lastname = contact.Lastname,
                Email = contact.Email,
                Category = contact.Category,
                SubCategory = contact.SubCategory,
                Phone = contact.Phone,
                BirthDate = contact.BirthDate
            };

            var hashedPassword = _passwordHasher.HashPassword(newContact, contact.Password);
            newContact.Password = hashedPassword;
            _dbContext.Contacts.Add(newContact);
            _dbContext.SaveChanges();

            return contact.Id;
        }

        public bool Update(int id, Contact c)
        {
            var contact = _dbContext.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact is null) return false;

            contact.Name = c.Name;
            contact.Lastname = c.Lastname;
            contact.Email = c.Email;
            contact.Category = c.Category;
            contact.SubCategory = c.SubCategory;
            contact.Phone = c.Phone;
            contact.BirthDate = c.BirthDate;


            var hashedPassword = _passwordHasher.HashPassword(contact, c.Password);
            contact.Password = hashedPassword;
            _dbContext.SaveChanges();
            return true;

        }

        public bool Delete(int id)
        {
            var contact = _dbContext.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact is null) return false;

            _dbContext.Contacts.Remove(contact);
            _dbContext.SaveChanges();
            return true;
        }

        public string GenerateJwt(LoginDto dto)
        {
            var contact = _dbContext.Contacts.FirstOrDefault(c => c.Email == dto.Email);

            if (contact is null) return null;

            var result = _passwordHasher.VerifyHashedPassword(contact, contact.Password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, contact.Id.ToString()),
                new Claim(ClaimTypes.Name, contact.Name.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(7);
            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
            
        }

    }
}
