using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Application.DTOs
{
    public class AgregarItemCarritoDto
    {
        public Guid ProductoId { get; set; }
        public int Cantidad { get; set; }
    }

}
