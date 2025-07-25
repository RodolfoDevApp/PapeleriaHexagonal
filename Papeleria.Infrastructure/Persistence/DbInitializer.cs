using Papeleria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Categorias.Any())
            {
                var papeleria = new Categoria("Papelería");
                var cereria = new Categoria("Cerería");
                var cursos = new Categoria("Cursos");

                context.Categorias.AddRange(papeleria, cereria, cursos);
                context.SaveChanges();

                // Productos con imágenes
                var prod1 = new Producto(
                    "Cuaderno Decorado",
                    "Cuaderno artesanal con portada personalizada",
                    150,
                    papeleria.Id,
                    2,
                    "https://placehold.co/300x200?text=Cuaderno"
                );

                var prod2 = new Producto(
                    "Vela Aromática",
                    "Vela de soya con esencia de lavanda",
                    180,
                    cereria.Id,
                    3,
                    "https://placehold.co/300x200?text=Vela"
                );

                var prod3 = new Producto(
                    "Set de Stickers",
                    "Stickers ilustrados para journaling",
                    70,
                    papeleria.Id,
                    1,
                    "https://placehold.co/300x200?text=Stickers"
                );

                context.Productos.AddRange(prod1, prod2, prod3);

                // Cursos con imágenes
                var curso1 = new Curso(
                    titulo: "Taller de Caligrafía",
                    descripcion: "Aprende lettering moderno con herramientas básicas",
                    precio: 350,
                    categoriaId: cursos.Id,
                    lugar: "Taller AR Creaciones - CDMX",
                    duracionHoras: 3,
                    fecha: new DateTime(2025, 8, 15),
                    imagenUrl: "https://placehold.co/300x200?text=Caligrafía"
                );

                var curso2 = new Curso(
                    titulo: "Curso de Cera Artesanal",
                    descripcion: "Crea tus propias velas desde casa",
                    precio: 500,
                    categoriaId: cursos.Id,
                    lugar: "Online (Zoom)",
                    duracionHoras: 3,
                    fecha: new DateTime(2025, 9, 5),
                    imagenUrl: "https://placehold.co/300x200?text=Cera+Artesanal"
                );

                context.Cursos.AddRange(curso1, curso2);

                context.SaveChanges();
            }
        }
    }
}
