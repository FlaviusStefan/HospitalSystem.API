using HospitalSystem.API.Models.Domain;
using HospitalSystem.API.Models.DTO;
using HospitalSystem.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressRepository addressRepository;

        public AddressesController(IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(CreateAddressRequestDto request)
        {
            var address = new Address
            {
                Id = Guid.NewGuid(),
                Street = request.Street,
                StreetNr = request.StreetNr,
                City = request.City,
                State = request.State,
                Country = request.Country,
                PostalCode = request.PostalCode
            };

            await addressRepository.CreateAsync(address);

            var response = new AddressDto
            {
                Id = address.Id,
                Street = address.Street,
                StreetNr = address.StreetNr,
                City = address.City,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            var addresses = await addressRepository.GetAllAsync();

            var response = addresses.Select(address => new AddressDto
            {
                Id = address.Id,
                Street = address.Street,
                StreetNr = address.StreetNr,
                City = address.City,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode
            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetAddressById([FromRoute] Guid id)
        {
            var existingAddress = await addressRepository.GetById(id);
            if(existingAddress == null)
            {
                return NotFound();
            }

            var response = new AddressDto
            {
                Id = existingAddress.Id,
                Street = existingAddress.Street,
                StreetNr = existingAddress.StreetNr,
                City = existingAddress.City,
                State = existingAddress.State,
                Country = existingAddress.Country,
                PostalCode = existingAddress.PostalCode
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateAddress([FromRoute] Guid id, UpdateAddressRequestDto request)
        {
            var existingAddress = await addressRepository.GetById(id);
            if( existingAddress == null) 
            {
                return NotFound();
            }
            existingAddress.Street = request.Street;
            existingAddress.StreetNr = request.StreetNr;
            existingAddress.City = request.City;
            existingAddress.State = request.State;
            existingAddress.Country = request.Country;
            existingAddress.PostalCode = request.PostalCode;

            var updatedAddress = await addressRepository.UpdateAsync(existingAddress);
            if(updatedAddress == null)
            {
                return NotFound();
            }

            var response = new AddressDto
            {
                Id = updatedAddress.Id,
                Street = updatedAddress.Street,
                StreetNr= updatedAddress.StreetNr,
                City = updatedAddress.City,
                State = updatedAddress.State,
                Country = updatedAddress.Country,
                PostalCode = updatedAddress.PostalCode
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAddress([FromRoute] Guid id)
        {
            var address = await addressRepository.DeleteAsync(id);

            if(address is null)
            {
                return NotFound();
            }

            var response = new AddressDto
            {
                Id = address.Id,
                Street = address.Street,
                StreetNr = address.StreetNr,
                City = address.City,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode
            };

            return Ok(response); 
        }
    }
}
