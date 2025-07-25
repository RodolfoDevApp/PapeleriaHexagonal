using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Domain.Entities
{
    public class Curso : AuditableEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Titulo { get; private set; } = string.Empty;

        public string Descripcion { get; private set; }= string.Empty;

        public decimal Precio { get; private set; }

        public string Lugar { get; private set; }= string.Empty;

        public DateTime Fecha { get; private set; }

        public int DuracionHoras { get; private set; }

        public Guid CategoriaId { get; private set; }
        public Categoria Categoria { get; private set; } = default!;

        public bool Activo { get; private set; } = true;
        public string ImagenUrl { get; private set; } = string.Empty;


        // Constructor requerido por EF
        private Curso() { }

        public Curso(string titulo, string descripcion, decimal precio, Guid categoriaId, string lugar, DateTime fecha, int duracionHoras, string imagenUrl)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            Precio = precio;
            Lugar = lugar;
            Fecha = fecha;
            DuracionHoras = duracionHoras;
            CategoriaId = categoriaId;
            Activo = true;
        }

        public void Actualizar(string titulo, string descripcion, decimal precio, Guid categoriaId, string lugar, DateTime fecha, int duracionHoras, string imagenUrl)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            Precio = precio;
            CategoriaId = categoriaId;
            Lugar = lugar;
            Fecha = fecha;
            DuracionHoras = duracionHoras;
            ImagenUrl = imagenUrl;
        }

        public void Cancelar() => Activo = false;

        public void Reactivar() => Activo = true;
    }
}
