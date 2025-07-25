using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Domain.Entities
{
    public class Producto : AuditableEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Nombre { get; private set; }= string.Empty;

        public string Descripcion { get; private set; }= string.Empty;

        public decimal Precio { get; private set; }

        public int TiempoElaboracionDias { get; private set; }

        public bool Activo { get; private set; } = true;
        public Guid CategoriaId { get; private set; }
        public Categoria Categoria { get; private set; } = default!;
        public string ImagenUrl { get; private set; } = string.Empty;


        // Constructor para EF
        private Producto() { }

        public Producto(string nombre, string descripcion, decimal precio, Guid categoriaId, int tiempoElaboracionDias, string imagenUrl)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            TiempoElaboracionDias = tiempoElaboracionDias;
            CategoriaId = categoriaId;
            Activo = true;
        }

        public void Actualizar(string nombre, string descripcion, decimal precio, Guid categoriaId, int tiempoElaboracionDias, string imagenUrl)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            TiempoElaboracionDias = tiempoElaboracionDias;
            ImagenUrl = imagenUrl;
            CategoriaId = categoriaId;
        }
        public void Desactivar() => Activo = false;
        public void Activar() => Activo = true;
    }
}
