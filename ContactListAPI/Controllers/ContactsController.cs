using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactListAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ContactListAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace ContactListAPI.Controllers
{
    [Route("api/contacts")]
    public class ContactsController : Controller
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        // Metoda GET do wyświetlenia wybranych informacji dla niezalogowanego użytkownika
        [HttpGet]

        public ActionResult<IEnumerable<ContactDto>> GetAll()
        {        
            var contactDto = _contactService.GetAll();

            return Ok(contactDto);

        }

        //Metoda GET mająca za zadanie wyświetlić wszystkie szczegóły o danym kontakcie
        [HttpGet("{id}")]
        [Authorize(Policy = "LoggedIn")]
        public ActionResult<Contact> Get([FromRoute] int id)
        {
            var contact = _contactService.GetById(id);

            return Ok(contact);

        }

        //Metoda POST służąca do stworzenia nowego kontaktu
        [HttpPost]
        [Authorize(Policy = "LoggedIn")]
        public ActionResult CreateContact([FromBody] Contact contact)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = _contactService.Create(contact);

            return Created($"/api/contact/{id}", null);
        }

        //Metoda PUT służąca do edytowania kontaktu
        [HttpPut("{id}")]
        [Authorize(Policy = "LoggedIn")]
        public ActionResult EditContact([FromBody] Contact c, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isUpdated = _contactService.Update(id, c);
            if (!isUpdated) return NotFound();

            return Ok();



        }

        //Metoda DELETE służąca do usunięcia danego kontaktu
        [HttpDelete("{id}")]
        [Authorize(Policy = "LoggedIn")]
        public ActionResult Delete([FromRoute] int id)
        {
            var isDeleted = _contactService.Delete(id);

            if (isDeleted) return NoContent();

            return NotFound();
        }

        //Metoda POST służąca do zalogowania się użytkownika
        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            string token = _contactService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}
