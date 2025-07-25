using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Domain.Entities
{
    public abstract class AuditableEntity
    {
        public DateTime FechaRegistro { get; private set; } = DateTime.UtcNow;
    }
}
