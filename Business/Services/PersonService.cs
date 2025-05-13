using Business.Interfaces;
using Business.Validations;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonService> _logger;

        public PersonService(IPersonRepository personRepository, ILogger<PersonService> logger)
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync()
        {
            return await _personRepository.GetAllAsync();
        }

        public async Task<Person?> GetPersonByIdAsync(int id)
        {
            return await _personRepository.GetByIdAsync(id);
        }

        public async Task<Person> CreatePersonAsync(Person person)
        {
            LogicValidations.PersonValidations.ValidatePerson(person); // Llama al método de validación
            // Aquí podrías agregar lógica de negocio antes de crear la persona
            person.active = true; // Aseguramos que las nuevas personas estén activas
            return await _personRepository.AddAsync(person);
        }

        public async Task<bool> UpdatePersonAsync(Person person)
        {
            LogicValidations.PersonValidations.ValidatePerson(person); // También puedes validar en la actualización
            // Aquí podrías agregar lógica de negocio antes de actualizar la persona
            return await _personRepository.UpdateAsync(person);
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            // Aquí podrías agregar lógica de negocio antes de eliminar la persona
            return await _personRepository.DeleteAsync(id);
        }
    }
}