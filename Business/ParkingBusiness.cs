using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business
{
    public class ParkingBusiness
    {
        private readonly ParkingData _parkingData;
        private readonly ILogger<ParkingBusiness> _logger;

        public ParkingBusiness(ParkingData parkingData, ILogger<ParkingBusiness> logger)
        {
            _parkingData = parkingData ?? throw new ArgumentNullException(nameof(parkingData));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Método de validación común
        private void ValidateParking(Parking parking)
        {
            if (string.IsNullOrEmpty(parking.name))
            {
                throw new ArgumentException("El nombre del parking no puede estar vacío.");
            }

            if (string.IsNullOrEmpty(parking.location))
            {
                throw new ArgumentException("La ubicación del parking no puede estar vacía.");
            }

            if (parking.id_camara <= 0) // Validación para id_camara
            {
                throw new ArgumentException("El ID de la cámara no es válido.");
            }

            if (string.IsNullOrEmpty(parking.hability)) // Validación de hability
            {
                throw new ArgumentException("El campo 'hability' no puede estar vacío.");
            }
        }

        // Obtener todos los parkings con LINQ
        public async Task<IEnumerable<Parking>> GetAllAsync()
        {
            try
            {
                return await _parkingData.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los parkings.");
                throw;
            }
        }

        // Obtener parking por ID con LINQ
        public async Task<Parking?> GetByIdAsync(int id)
        {
            try
            {
                return await _parkingData.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el parking con ID {ParkingId}.", id);
                throw;
            }
        }

        // Crear un nuevo parking
        public async Task<Parking> CreateAsync(Parking parking)
        {
            try
            {
                // Validación de negocio antes de crear el parking
                ValidateParking(parking);

                // Si el campo 'hability' es nulo o vacío, asigna un valor predeterminado
                if (string.IsNullOrEmpty(parking.hability))
                {
                    parking.hability = "default_value"; // Asignar un valor predeterminado adecuado
                }

                return await _parkingData.CreateAsync(parking);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al crear el parking.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el parking.");
                throw;
            }
        }

        // Actualizar parking existente
        public async Task<bool> UpdateAsync(Parking parking)
        {
            try
            {
                // Validación de negocio antes de actualizar el parking
                if (parking.id <= 0)
                {
                    throw new ArgumentException("El ID del parking no es válido.");
                }

                ValidateParking(parking);

                // Si el campo 'hability' es nulo o vacío, asigna un valor predeterminado
                if (string.IsNullOrEmpty(parking.hability))
                {
                    parking.hability = "default_value"; // Asignar un valor predeterminado adecuado
                }

                return await _parkingData.UpdateAsync(parking);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al actualizar el parking.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el parking.");
                return false;
            }
        }

        // Eliminar parking
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var parking = await _parkingData.GetByIdAsync(id);
                if (parking == null)
                {
                    throw new Exception($"El parking con ID {id} no existe.");
                }

                return await _parkingData.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el parking con ID {ParkingId}.", id);
                throw;
            }
        }

        // Métodos SQL
        // [Los métodos SQL son muy similares, solo cambia el tipo de consulta, y siguen la misma estructura de validación]
    }
}
