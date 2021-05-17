using System.Collections.Generic;
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
    [Route("/pets")]
    public class PetsController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly AddressRepository addressRepository;
        private readonly CityRepository cityRepository;
        private readonly CaretakerRepository caretakerRepository;
        private readonly VetRepository vetRepository;
        private readonly PetRepository<Pet> petRepository;
        private readonly PetRepository<Cat> catRepository;
        private readonly PetRepository<Dog> dogRepository;

        public PetsController(
            UnitOfWork unitOfWork, 
            AddressRepository addressRepository, 
            CityRepository cityRepository,
            CaretakerRepository caretakerRepository,
            VetRepository vetRepository,
            PetRepository<Pet> petRepository,
            PetRepository<Cat> catRepository,
            PetRepository<Dog> dogRepository)
        {
            this.unitOfWork = unitOfWork;
            this.addressRepository = addressRepository;
            this.cityRepository = cityRepository;
            this.caretakerRepository = caretakerRepository;
            this.vetRepository = vetRepository;
            this.petRepository = petRepository;
            this.catRepository = catRepository;
            this.dogRepository = dogRepository;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<BaseQueryResult<Pet>>> GetPets()
        {
            var query = new BaseQueryModel {
                PageSize = -1
            };
            var pets = await this.addressRepository.GetAllAsync(query, true);
            return Ok(pets);
        }

        [HttpGet("get/id")]
        public async Task<ActionResult<Pet>> GetPet(long id)
        {
            var pet = await this.addressRepository.GetAsync(id, true);
            return Ok(pet);
        }

        #region Add pets
        [HttpPost("pet/add")]
        public async Task<ActionResult<Pet>> AddPet([FromBody]PetResource pet) {
            List<Caretaker> caretakers = new List<Caretaker>();
            foreach (long id in pet.caretakerIds)
            {
                var caretaker = await this.caretakerRepository.GetAsync(id);
                if(caretaker == null) return NotFound($"Caretaker with id {id} Not Found");
                caretakers.Add(caretaker);
            }

            var vet = await this.vetRepository.GetAsync(pet.vetId);
            if(vet == null) return NotFound("Vet Not Found");

            Pet newPet = new Pet 
            {
                name = pet.name,
                age = pet.age,
                vet = vet,
                caretakers = caretakers
            };

            this.petRepository.Add(newPet);
            await this.unitOfWork.CompletedAsync();

            return Ok(newPet);
        }

        [HttpPost("dog/add")]
        public async Task<ActionResult<Dog>> AddDog([FromBody]DogResource dog) {
            List<Caretaker> caretakers = new List<Caretaker>();
            foreach (long id in dog.caretakerIds)
            {
                var caretaker = await this.caretakerRepository.GetAsync(id);
                if(caretaker == null) return NotFound($"Caretaker with id {id} Not Found");
                caretakers.Add(caretaker);
            }

            var vet = await this.vetRepository.GetAsync(dog.vetId);
            if(vet == null) return NotFound("Vet Not Found");

            Dog newDog = new Dog 
            {
                name = dog.name,
                age = dog.age,
                barkPitch = dog.barkPitch,
                vet = vet,
                caretakers = caretakers
            };

            this.petRepository.Add(newDog);
            await this.unitOfWork.CompletedAsync();

            return Ok(newDog);
        }

        [HttpPost("cat/add")]
        public async Task<ActionResult<Cat>> AddCat([FromBody]CatResource cat) {
            List<Caretaker> caretakers = new List<Caretaker>();
            foreach (long id in cat.caretakerIds)
            {
                var caretaker = await this.caretakerRepository.GetAsync(id);
                if(caretaker == null) return NotFound($"Caretaker with id {id} Not Found");
                caretakers.Add(caretaker);
            }

            var vet = await this.vetRepository.GetAsync(cat.vetId);
            if(vet == null) return NotFound("Vet Not Found");

            Cat newCat = new Cat 
            {
                name = cat.name,
                age = cat.age,
                liveCount = cat.liveCount,
                vet = vet,
                caretakers = caretakers
            };

            this.petRepository.Add(newCat);
            await this.unitOfWork.CompletedAsync();

            return Ok(newCat);
        }
        #endregion
    }
}
