using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Infrastructure.Security
{
    public class JwtSettings
    {
        public string ClaveSecreta { get; set; } = string.Empty;
        public string Emisor { get; set; } = string.Empty;
        public string Audiencia { get; set; } = string.Empty;
        public int ExpiracionMinutos { get; set; }
    }
}
