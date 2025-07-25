using Papeleria.Application.DTOs;
using Papeleria.WebApi.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Application.Interfaces
{
    public interface IFacturaService
    {
        Task<ResultDto<FacturaDto>> ConfirmarCompraAsync(Guid usuarioId);
    }

}
