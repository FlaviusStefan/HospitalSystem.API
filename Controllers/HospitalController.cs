using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalRepository hospitalRepository;
        private readonly IAddressRepository addressRepository;
        private readonly IContactRepository contactRepository;

        public HospitalController(IHospitalRepository hospitalRepository, IAddressRepository addressRepository, IContactRepository contactRepository)
        {
            this.hospitalRepository = hospitalRepository;
            this.addressRepository = addressRepository;
            this.contactRepository = contactRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHospital(CreateHospitalRequestDto request)
        {
            // Create Address entity
            var address = new Address
            {
                Id = Guid.NewGuid(),
                Street = request.Address.Street,
                StreetNr = request.Address.StreetNr,
                City = request.Address.City,
                State = request.Address.State,
                Country = request.Address.Country,
                PostalCode = request.Address.PostalCode
            };

            // Create Contact entity
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                Phone = request.Contact.Phone,
                Email = request.Contact.Email
            };

            await addressRepository.CreateAsync(address);
            await contactRepository.CreateAsync(contact);

            var hospital = new Hospital
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                AddressId = address.Id,
                ContactId = contact.Id
            };

            await hospitalRepository.CreateAsync(hospital);

            var response = new HospitalDto
            {
                Id = hospital.Id,
                Name = hospital.Name,
                AddressId = hospital.AddressId,
                ContactId = hospital.ContactId
            };

            return Ok(response);

        }
    }
}
