// Business/Services/PersonService.cs

using Business.Interfaces;
using Business.Validations; // Para tus validaciones de negocio
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper; // Necesario para AutoMapper
using Entity.DTOs;

namespace Business.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonService> _logger;
        private readonly IMapper _mapper;

        public PersonService(
            IPersonRepository personRepository,
            ILogger<PersonService> logger,
            IMapper mapper)
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PersonDTO>> GetAllPersonsAsync()
        {
            _logger.LogInformation("Obteniendo todas las personas para DTOs.");
            var persons = await _personRepository.GetAllAsync(); // Obtiene entidades Person
            return _mapper.Map<IEnumerable<PersonDTO>>(persons); // Mapea la colección de entidades a DTOs
        }

        public async Task<PersonDTO?> GetPersonByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo persona con ID {Id} para DTO.", id);
            var person = await _personRepository.GetByIdAsync(id); // Obtiene la entidad Person
            if (person == null)
            {
                _logger.LogWarning("Persona con ID {Id} no encontrada en el repositorio.", id);
                return null;
            }
            return _mapper.Map<PersonDTO>(person); // Mapea la entidad a DTO
        }

        public async Task<PersonDTO> CreatePersonAsync(PersonCreateUpdateDTO personCreateDto)
        {
            _logger.LogInformation("Creando nueva persona desde DTO.");

            // Validaciones de negocio (ej. documento único, email único)
            // Asumo que LogicValidations.PersonValidations.ValidatePerson ya está adaptado
            // para trabajar con un DTO o que se puede aplicar después del mapeo inicial.
            // Si ValidatePerson espera una entidad Person, podrías mapear primero y luego validar.
            var person = _mapper.Map<Person>(personCreateDto); // Mapea DTO de creación a entidad

            // Llama al método de validación con la entidad mapeada
            // LogicValidations.PersonValidations.ValidatePerson(person); // Asegúrate de que esta validación maneje excepciones

            person.Active = personCreateDto.Active; // Usar el valor de Active del DTO

            var createdPerson = await _personRepository.AddAsync(person); // Guarda la entidad
            _logger.LogInformation("Persona con ID {Id} creada en la base de datos.", createdPerson.Id);

            return _mapper.Map<PersonDTO>(createdPerson); // Mapea la entidad creada a DTO para devolver
        }

        public async Task<bool> UpdatePersonAsync(int id, PersonCreateUpdateDTO personUpdateDto)
        {
            _logger.LogInformation("Actualizando persona con ID {Id} desde DTO.", id);
            var existingPerson = await _personRepository.GetByIdAsync(id);
            if (existingPerson == null)
            {
                _logger.LogWarning("Intento de actualizar persona con ID {Id} falló: no encontrada.", id);
                return false;
            }

            // Validaciones de negocio antes de actualizar
            // Si el documento o email se cambia, asegurar unicidad
            // LogicValidations.PersonValidations.ValidatePerson(personUpdateDto); // Si tienes validaciones para DTOs

            // Mapear los campos actualizables del DTO a la entidad existente
            _mapper.Map(personUpdateDto, existingPerson); // AutoMapper actualizará las propiedades coincidentes

            var result = await _personRepository.UpdateAsync(existingPerson);
            if (result)
            {
                _logger.LogInformation("Persona con ID {Id} actualizada exitosamente.", id);
            }
            else
            {
                _logger.LogError("Error al actualizar persona con ID {Id} en el repositorio.", id);
            }
            return result;
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            _logger.LogInformation("Realizando borrado lógico de persona con ID {Id}.", id);
            var personToDelete = await _personRepository.GetByIdAsync(id);
            if (personToDelete == null)
            {
                _logger.LogWarning("Intento de borrado lógico de persona con ID {Id} falló: no encontrada.", id);
                return false;
            }

            personToDelete.Active = false; // Borrado lógico: marcar como inactivo
            var result = await _personRepository.UpdateAsync(personToDelete);
            if (result)
            {
                _logger.LogInformation("Persona con ID {Id} eliminada lógicamente exitosamente.", id);
            }
            else
            {
                _logger.LogError("Error al realizar borrado lógico de persona con ID {Id} en el repositorio.", id);
            }
            return result;
        }

        // CONSIDERACIONES:
        // - LogicValidations.PersonValidations: Asegúrate de que los métodos en esta clase
        //   lancen excepciones claras que puedan ser manejadas por tu middleware global de errores,
        //   en lugar de solo loggear advertencias si la validación es crítica para el flujo.
    }
}