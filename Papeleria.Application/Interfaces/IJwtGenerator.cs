using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Application.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerarToken(Guid usuarioId, string nombre, string email, string rol);
    }
}
