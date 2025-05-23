using Business.Services;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.Exceptions;
namespace Business.Validations;

public class LogicValidations
{
    //Validación para el nombre de usuario
    public static void EnsureUserNameIsUnique(IEnumerable<User> users, string username)
    {
        if (users.Any(u => u.Username == username))
            throw new ValidationException("El nombre de usuario ya está registrado.");
    }

    public static void EnsureUserExists(User? user)
    {
        if (user == null)
            throw new ValidationException("El usuario no existe.");
    }

    public static void ValidateRolName(string name, ILogger logger)
    {
        if (string.IsNullOrEmpty(name))
        {
            logger.LogError("El nombre del rol no puede ser nulo o vacío.");
            throw new ArgumentException("El nombre del rol es requerido.", nameof(name));
        }
    }

    // Validación para la descripción del rol
    public static void ValidateRolDescription(string description, ILogger logger)
    {
        if (string.IsNullOrEmpty(description))
        {
            logger.LogError("La descripción del rol no puede ser nulo o vacío.");
            throw new ArgumentException("La descripción del rol es requerida.", nameof(description));
        }
    }

    // Validación para la existencia del rol
    public static void ValidateExistingRol(Rol? rol, int id, ILogger logger)
    {
        if (rol == null)
        {
            logger.LogError($"No se encontró el rol con ID: {id}.");
            throw new NotFoundException($"Rol con ID {id} no encontrado."); // Asegúrate de tener esta excepción o usa una adecuada.
        }
    }

    // Validación para el nombre del permiso
    public static void ValidatePermissionName(string name, ILogger logger)
    {
        if (string.IsNullOrEmpty(name))
        {
            logger.LogError("El nombre del permiso no puede ser nulo o vacío.");
            throw new ArgumentException("El nombre del permiso es requerido.", nameof(name));
        }
    }

    // Validación para la existencia del permiso
    public static void ValidateExistingPermission(Permission? permission, int id, ILogger logger)
    {
        if (permission == null)
        {
            logger.LogError($"No se encontró el permiso con ID: {id}.");
            throw new NotFoundException($"Permiso con ID {id} no encontrado."); // Asegúrate de tener esta excepción o usa una adecuada.
        }
    }


    // Validación para el nombre del módulo
    public static void ValidateModuleName(string name, ILogger logger)
    {
        if (string.IsNullOrEmpty(name))
        {
            logger.LogError("El nombre del módulo no puede ser nulo o vacío.");
            throw new ArgumentException("El nombre del módulo es requerido.", nameof(name));
        }
    }

    // Validación para la existencia del módulo
    public static void ValidateExistingModule(Modules? module, int id, ILogger logger)
    {
        if (module == null)
        {
            logger.LogError($"No se encontró el módulo con ID: {id}.");
            throw new NotFoundException($"Módulo con ID {id} no encontrado."); // Asegúrate de tener esta excepción o usa una adecuada.
        }
    }

    // Validación para FormModule
    public static void ValidateFormModule(FormModuleCreateDTO formModule, ILogger logger)
    {
        if (formModule.FormId <= 0)
        {
            logger.LogError("El ID del formulario debe ser mayor que cero.");
            throw new ArgumentException("El ID del formulario debe ser mayor que cero.", nameof(formModule.FormId));
        }

        if (formModule.ModuleId <= 0)
        {
            logger.LogError("El ID del módulo debe ser mayor que cero.");
            throw new ArgumentException("El ID del módulo debe ser mayor que cero.", nameof(formModule.ModuleId));
        }
    }

    public static void ValidateExistingFormModule(FormModuleDTO? formModule, int id, ILogger logger)
    {
        if (formModule == null)
        {
            logger.LogError($"No se encontró la relación Form-Módulo con ID: {id}.");
            throw new NotFoundException($"Relación Form-Módulo con ID {id} no encontrado.");
        }
    }

    internal static void ValidateExistingFormModule(FormModule? existingFormModule, int id, ILogger<FormModuleService> logger)
    {
        throw new NotImplementedException();
    }

    public static class PersonValidations
    {
        public static void ValidatePerson(Person person)
        {
            if (!IsValidName(person.Name))
            {
                throw new NotFoundException("El nombre no es válido.");
            }
            if (!IsValidLastName(person.Lastname))
            {
                throw new NotFoundException("El apellido no es válido.");
            }
            if (!IsValidDocument(person.Document))
            {
                throw new NotFoundException("El documento no es válido.");
            }
            if (!IsValidPhone(person.Phone))
            {
                throw new NotFoundException("El teléfono no es válido.");
            }
            if (!IsValidEmail(person.Email))
            {
                throw new NotFoundException("El correo electrónico no es válido.");
            }
            // Puedes agregar más validaciones aquí
        }

        public static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length <= 100 && Regex.IsMatch(name, "^[a-zA-ZáéíóúüñÁÉÍÓÚÜÑ\\s]+$");
        }

        public static bool IsValidLastName(string lastname)
        {
            return !string.IsNullOrWhiteSpace(lastname) && lastname.Length <= 100 && Regex.IsMatch(lastname, "^[a-zA-ZáéíóúüñÁÉÍÓÚÜÑ\\s]+$");
        }

        public static bool IsValidDocument(string document)
        {
            return !string.IsNullOrWhiteSpace(document) && document.Length <= 20 && Regex.IsMatch(document, "^[a-zA-Z0-9-]+$");
        }

        public static bool IsValidPhone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) && phone.Length <= 20 && Regex.IsMatch(phone, "^[0-9+-]+$");
        }

        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }

    public static class FormValidations
    {
        public static void ValidateForm(Forms form)
        {
            if (!IsValidFormName(form.Name))
            {
                throw new ArgumentException("El nombre del formulario no es válido.");
            }
            if (!IsValidFormUrl(form.Url))
            {
                throw new ArgumentException("La URL del formulario no es válida.");
            }
            // Puedes agregar más validaciones aquí
        }

        public static bool IsValidFormName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length <= 200; // Ejemplo de validación
        }

        public static bool IsValidFormUrl(string url)
        {
            return !string.IsNullOrWhiteSpace(url) && url.Length <= 500 && Uri.IsWellFormedUriString(url, UriKind.Absolute); // Ejemplo de validación de URL
        }
    }
}
