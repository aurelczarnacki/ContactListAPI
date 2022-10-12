using AutoMapper;
using ContactListAPI.Models;

namespace ContactListAPI
{
    public class ContactsMappingProfile : Profile
    {
        public ContactsMappingProfile()
        {
            CreateMap<Contact, ContactDto>();

            CreateMap<LoginDto, Contact>();
        }
    }
}
