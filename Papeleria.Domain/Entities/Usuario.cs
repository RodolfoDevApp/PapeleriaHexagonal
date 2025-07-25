using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papeleria.Domain.Entities
{
    public class Usuario : AuditableEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Nombre { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string Rol { get; private set; } = "Cliente";

        public string? TokenRecuperacion { get; private set; }
        public DateTime? ExpiraToken { get; private set; }

        public bool Activo { get; private set; } = true;
        public string? TokenVerificacion { get; private set; }
        public DateTime? ExpiraVerificacion { get; private set; }

        // Constructor requerido por EF
        private Usuario() { }

        public Usuario(string nombre, string email, string passwordHash, string rol = "Cliente")
        {
            Nombre = nombre;
            Email = email;
            PasswordHash = passwordHash;
            Rol = rol;
            Activo = true;
        }

        public void Desactivar() => Activo = false;
        public void Activar() => Activo = true;

        public void CambiarPassword(string nuevoHash)
        {
            PasswordHash = nuevoHash;
        }

        public void GenerarTokenRecuperacion(string token, DateTime expiracion)
        {
            TokenRecuperacion = token;
            ExpiraToken = expiracion;
        }

        public void LimpiarTokenRecuperacion()
        {
            TokenRecuperacion = null;
            ExpiraToken = null;
        }

        public void Actualizar(string nombre, string email, string rol)
        {
            Nombre = nombre;
            Email = email;
            Rol = rol;
        }

        public void CambiarRol(string nuevoRol)
        {
            Rol = nuevoRol;
        }
        public void GenerarTokenVerificacion(string token, DateTime expiracion)
        {
            TokenVerificacion = token;
            ExpiraVerificacion = expiracion;
        }

        public void VerificarCuenta()
        {
            Activo = true;
            TokenVerificacion = null;
            ExpiraVerificacion = null;
        }
    }
}
