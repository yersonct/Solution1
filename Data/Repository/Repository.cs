using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IApplicationDbContextWithEntry _context; // Cambiado a IApplicationDbContext

        public Repository(IApplicationDbContextWithEntry context) // Cambiado a IApplicationDbContext
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al agregar la entidad", ex);
            }
        }

        // Eliminar una entidad de la base de datos
        public void Delete(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);  // Elimina la entidad
                _context.SaveChangesAsync();           // Guarda los cambios
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new InvalidOperationException("Error al eliminar la entidad", ex);
            }
        }

        // Obtener todas las entidades de la base de datos
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();  // Obtiene todas las entidades
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new InvalidOperationException("Error al obtener las entidades", ex);
            }
        }

        // Obtener una entidad por su ID
        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);  // Busca por ID
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new InvalidOperationException("Error al obtener la entidad por ID", ex);
            }
        }

        // Guardar los cambios en la base de datos
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();  // Guarda los cambios
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new InvalidOperationException("Error al guardar los cambios", ex);
            }
        }

        // Actualizar una entidad en la base de datos
        public void Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);  // Actualiza la entidad
                _context.SaveChangesAsync();           // Guarda los cambios
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new InvalidOperationException("Error al actualizar la entidad", ex);
            }
        }
    }
}