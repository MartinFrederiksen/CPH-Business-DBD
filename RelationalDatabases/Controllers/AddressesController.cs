using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RelationalDatabases.Controllers.Resources;
using RelationalDatabases.DataModels;
using RelationalDatabases.DataModels.QueryModels;
using RelationalDatabases.Persistence;
using RelationalDatabases.Persistence.Repositories;

namespace RelationalDatabases.Controllers
{
    [ApiController]
    [Route("/address")]
    public class AddressesController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly AddressRepository addressRepository;
        private readonly CityRepository cityRepository;

        public AddressesController(
            UnitOfWork unitOfWork, 
            AddressRepository addressRepository, 
            CityRepository cityRepository)
        {
            this.unitOfWork = unitOfWork;
            this.addressRepository = addressRepository;
            this.cityRepository = cityRepository;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<BaseQueryResult<Address>>> GetAddresses()
        {
            var query = new BaseQueryModel {
                PageSize = -1
            };
            var addresses = await this.addressRepository.GetAllAsync(query, true);
            return Ok(addresses);
        }

        [HttpGet("get/id")]
        public async Task<ActionResult<Address>> GetAddress(long id)
        {
            var address = await this.addressRepository.GetAsync(id, true);
            return Ok(address);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Address>> AddAddress([FromBody]AddressResource address) {
            var city = await this.cityRepository.GetByZipAsync(address.zip);
            if(city == null) 
                city = new City {
                    zip = address.zip,
                    city = address.city
                };
            Address newAddress = new Address
            {
                street = address.street,
                housenumber = address.housenumber,
                city = city
            };

            this.addressRepository.Add(newAddress);
            await this.unitOfWork.CompletedAsync();

            return Ok(newAddress);
        }
    }
}
