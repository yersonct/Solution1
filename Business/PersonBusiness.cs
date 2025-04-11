using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class PersonBusiness
    {
        private readonly PersonData _personData;
        private readonly ILogger<PersonBusiness> _logger;

        public PersonBusiness(PersonData personData, ILogger<PersonBusiness> logger)
        {
            _personData = personData ?? throw new ArgumentNullException(nameof(personData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<PersonDTO>> GetAllPersonsAsync()
        {
            try
            {
                var person = await _personData.GetAllAsyncSQL();

                return MapToDtoList(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los personas.");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de persona.", ex);
            }
        }

        public async Task<PersonDTO> GetPersonByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID del usuario debe ser mayor que cero.");
            }

            try
            {
                var person = await _personData.GetByIdAsyncSQL(id);
                if (person == null)
                {
                    throw new EntityNotFoundException("person", id);
                }
                return MapToDTO(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {PersonId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}.", ex);
            }
        }

        public async Task<PersonDTO> CreatePersonAsync(PersonDTO personDTO)
        {
            try
            {
                ValidatePerson(personDTO);
                var persona = MapToEntity(personDTO);
                var createdPerson = await _personData.CreateAsyncSQL(persona);
                return MapToDTO(createdPerson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo usuario: {Personname}", personDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario.", ex);
            }
        }

        public async Task<bool> UpdatePersonAsync(PersonDTO personDTO)
        {
            try
            {
                ValidatePerson(personDTO);
                var existingPerson = await _personData.GetByIdAsyncSQL(personDTO.Id);
                if (existingPerson == null)
                {
                    throw new EntityNotFoundException("Person", personDTO.Id);
                }

                existingPerson.name = personDTO.Name;
                existingPerson.lastname = personDTO.LastName;
                existingPerson.document = personDTO.Document;
                existingPerson.phone = personDTO.Phone;
                //existingPerson.age = personDTO.Age;
                existingPerson.email = personDTO.Email;
                existingPerson.active = personDTO.Active;

                return await _personData.UpdateAsyncSQL(existingPerson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID: {PersonId}", personDTO.Id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el usuario.", ex);
            }
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            try
            {
                var existingPerson = await _personData.GetByIdAsyncSQL(id);
                if (existingPerson == null)
                {
                    throw new EntityNotFoundException("Person", id);
                }
                return await _personData.DeleteAsyncSQL(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID: {PersonId}", id);
                throw new ExternalServiceException("Base de datos", "Error al eliminar el Person.", ex);
            }
        }

        private void ValidatePerson(PersonDTO personDTO)
        {
            if (personDTO == null)
            {
                throw new ValidationException("El objeto personas no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(personDTO.Name))
            {
                throw new ValidationException("Name", "El nombre de usuario es obligatorio.");
            }
        }

        private PersonDTO MapToDTO(Person person)
        {
            return new PersonDTO
            {
                Id = person.id,
                Name = person.name,
                LastName = person.lastname,
                Document = person.document,
                Phone = person.phone,
                //Age = person.age,
                Email = person.email,
                Active = person.active
               
            };
        }

        private Person MapToEntity(PersonDTO personDTO)
        {
            return new Person
            {
                id = personDTO.Id,
                name = personDTO.Name,
                lastname = personDTO.LastName,
                document = personDTO.Document,
                phone = personDTO.Phone,
                //age = personDTO.Age,
                email = personDTO.Email,
                active = personDTO.Active 
            };
        }
        private IEnumerable<PersonDTO> MapToDtoList(IEnumerable<Person> persons)
        {
            var personDto = new List<PersonDTO>();
            foreach (var person in persons)
            {
                personDto.Add(MapToDTO(person));
            }
            return personDto;
        }


    }
}
