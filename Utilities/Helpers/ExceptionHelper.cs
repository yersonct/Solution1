using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Helpers
{
    public class ExceptionHelper
    {
        public static void ValidateNotFound<T>(T entity, string message)
        {
            if (entity == null)
            {
                throw new NotFoundException(message);
            }
        }
        public static void ValidateNotNull<T>(T entity, string message)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(message);
            }
        }
        public static void ValidateNotEmpty(string value, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(message);
            }
        }
        public static void ValidateNotEmpty<T>(IEnumerable<T> collection, string message)
        {
            if (collection == null || !collection.Any())
            {
                throw new ArgumentException(message);
            }
        }
    }
}
