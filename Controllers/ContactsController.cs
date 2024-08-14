using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository contactRepository;

        public ContactsController(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(CreateContactRequestDto request)
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                Phone = request.Phone,
                Email = request.Email
            };

            await contactRepository.CreateAsync(contact);

            var response = new ContactDto
            {
                Id = contact.Id,
                Phone = contact.Phone,
                Email = contact.Email
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await contactRepository.GetAllAsync();

            var response = contacts.Select(contact => new ContactDto
            { 
                Id = contact.Id,
                Phone = contact.Phone,
                Email = contact.Email
            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetContactById([FromRoute] Guid id)
        {
            var existingContact = await contactRepository.GetById(id);
            if(existingContact == null)
            {
                return NotFound();
            }

            var response = new ContactDto
            {
                Id = existingContact.Id,
                Phone = existingContact.Phone,
                Email = existingContact.Email
            };

            return Ok(response);
            /* */
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id,UpdateContactRequestDto request)
        {
            var existingContact = await contactRepository.GetById(id);
            if(existingContact == null)
            {
                return NotFound();
            }

            existingContact.Phone = request.Phone;
            existingContact.Email = request.Email;

            var updatedContact = await contactRepository.UpdateAsync(existingContact);
            if(updatedContact == null)
            {
                return NotFound();
            }

            var response = new ContactDto
            {
                Id = updatedContact.Id,
                Phone = updatedContact.Phone,
                Email = updatedContact.Email
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await contactRepository.DeleteAsync(id);

            if(contact is null) 
            {
                return NotFound();
            }

            var response = new ContactDto
            {
                Id = contact.Id,
                Phone = contact.Phone,
                Email = contact.Email
            };

            return Ok(response);
        }
    }
}

