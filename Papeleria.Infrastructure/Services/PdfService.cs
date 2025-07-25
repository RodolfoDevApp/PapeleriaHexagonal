using HtmlRendererCore.PdfSharp;
using Microsoft.EntityFrameworkCore;
using Papeleria.Application.Interfaces;
using Papeleria.Domain.Entities;
using Papeleria.Infrastructure.Persistence;
using System.Text;
namespace Papeleria.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        private readonly AppDbContext _appDbContext;
        public PdfService(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }
        public async Task<byte[]> GenerarFacturaPdfAsync(Guid facturaId)
        {
            try
            {
                var factura = await _appDbContext.Facturas
        .Include(f => f.Detalles)
        .ThenInclude(d => d.Producto)
        .FirstOrDefaultAsync(f => f.Id == facturaId);

                if (factura == null)
                    throw new Exception("Factura no encontrada.");
                var html = GenerarHtmlFactura(factura);

                // Genera el PDF en tamaño A4
                var pdf = PdfGenerator.GeneratePdf(html, PdfSharpCore.PageSize.A4);

                using var stream = new MemoryStream();
                pdf.Save(stream);

                return await Task.FromResult(stream.ToArray());
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error o lanzar una excepción personalizada
                // Por ejemplo:
                throw new ApplicationException("Error al generar el PDF de la factura.", ex);
            }
        }



        private string GenerarHtmlFactura(FacturaHeader factura)
        {
            var sb = new StringBuilder();

            sb.Append($@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{
            font-family: Arial, sans-serif;
            padding: 20px;
            font-size: 12px;
            position: relative;
        }}
        header {{
            display: flex;
            align-items: center;
            margin-bottom: 20px;
        }}
        header img {{
            height: 60px;
            margin-right: 15px;
        }}
        h1 {{
            margin: 0;
            color: #444;
        }}
        h2 {{
            text-align: right;
            margin-top: 20px;
            color: #222;
        }}
        p {{
            margin: 4px 0;
        }}
        table {{
            width: 100%;
            border-collapse: collapse;
            margin-top: 15px;
        }}
        th, td {{
            border: 1px solid #ddd;
            padding: 8px;
        }}
        th {{
            background-color: #f2f2f2;
            text-align: left;
        }}
        tr:nth-child(even) {{
            background-color: #f9f9f9;
        }}
        footer {{
            position: fixed;
            bottom: 20px;
            left: 20px;
            right: 20px;
            text-align: center;
            font-size: 10px;
            color: #888;
        }}
    </style>
</head>
<body>

    <header>
        <img src='https://via.placeholder.com/150x60?text=LOGO' alt='Logo Empresa' />
        <div>
            <h1>Factura #{factura.Id}</h1>
            <p><strong>Fecha:</strong> {factura.Fecha:yyyy-MM-dd HH:mm:ss}</p>
            <p><strong>Cliente:</strong> {factura.UsuarioId}</p>
        </div>
    </header>

    <table>
        <thead>
            <tr>
                <th>Producto</th>
                <th>Cantidad</th>
                <th>Precio Unitario</th>
                <th>Subtotal</th>
            </tr>
        </thead>
        <tbody>");

            foreach (var item in factura.Detalles)
            {
                sb.Append($@"
            <tr>
                <td>{item.Producto?.Nombre ?? "Producto"}</td>
                <td>{item.Cantidad}</td>
                <td>${item.PrecioUnitario:F2}</td>
                <td>${item.Subtotal:F2}</td>
            </tr>");
            }

            sb.Append($@"
        </tbody>
    </table>

    <h2>Total: ${factura.Total:F2}</h2>

    <footer>
        AR Creaciones · contacto@arcreaciones.com · +52 55 1234 5678 · www.arcreaciones.com
    </footer>

</body>
</html>");

            return sb.ToString();
        }

    }
}
