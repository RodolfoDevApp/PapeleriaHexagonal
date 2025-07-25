using Microsoft.Extensions.DependencyInjection;
using Papeleria.Application.Interfaces;
using Papeleria.Infrastructure.Security;
using Papeleria.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Infrastructure.Configuration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<ICursoService, CursoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<ICorreoService, CorreoService>();
            services.AddScoped<IFacturaService, FacturaService>();
            services.AddScoped<ICarroService, CarroService>();
            services.AddScoped<IPdfService, PdfService>();
            return services;
        }
    }
}
