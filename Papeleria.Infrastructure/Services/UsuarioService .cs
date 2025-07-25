using Microsoft.EntityFrameworkCore;
using Papeleria.Application.DTOs;
using Papeleria.Application.Interfaces;
using Papeleria.Application.Settings;
using Papeleria.Domain.Entities;
using Papeleria.Infrastructure.Persistence;
using Papeleria.WebApi.DTOs;
using System.Security.Cryptography;
using System.Text;



namespace Papeleria.Infrastructure.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly ICorreoService _icorreoService;
        private readonly BackendSettings _backendSettings;
        public UsuarioService(AppDbContext context, IJwtGenerator jwtGenerator, ICorreoService icorreoService, BackendSettings backendSettings)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
            _icorreoService = icorreoService;
            _backendSettings = backendSettings;
        }

        public async Task<ResultDto<string>> RegistrarAsync(UsuarioRegistroDto dto)
        {
            try
            {
                if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                    return ResultDto<string>.FailureResult("El correo ya está registrado.");

                var hash = ComputeHash(dto.Password);
                var usuario = new Usuario(dto.Nombre, dto.Email, hash);

                // Generar token de verificación
                var token = Guid.NewGuid().ToString();
                var expiracion = DateTime.UtcNow.AddHours(24);
                usuario.GenerarTokenVerificacion(token, expiracion);

                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();

                // Construir enlace de verificación usando BaseUrl
                var link = $"{_backendSettings.BaseUrl}/api/usuarios/verificar?token={token}";

                await _icorreoService.EnviarCorreoAsync(dto.Email, "Verifica tu cuenta", $"Haz clic aquí para verificar tu cuenta: <a href='{link}'>Verificar</a>");

                return ResultDto<string>.SuccessResult("Registro exitoso.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult($"Error al registrar usuario: {ex.Message}");
            }
        }

        public async Task<ResultDto<string>> LoginAsync(UsuarioLoginDto dto)
        {
            try
            {
                var hash = ComputeHash(dto.Password);
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                    u.Email == dto.Email && u.PasswordHash == hash && u.Activo);

                if (usuario is null)
                    return ResultDto<string>.FailureResult("Credenciales inválidas.");

                var token = _jwtGenerator.GenerarToken(usuario.Id, usuario.Nombre, usuario.Email, usuario.Rol);
                return ResultDto<string>.SuccessResult(token, "Inicio de sesión exitoso.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error en el inicio de sesión." + ex.Message);
            }
        }

        public async Task<ResultDto<string>> CambiarPasswordAsync(Guid usuarioId, CambioPasswordDto dto)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario == null)
                    return ResultDto<string>.FailureResult("Usuario no encontrado.");

                var nuevoHash = ComputeHash(dto.NuevoPassword);
                usuario.CambiarPassword(nuevoHash);
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Contraseña actualizada.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al cambiar contraseña." + ex.Message);
            }
        }

        public async Task<ResultDto<string>> EnviarRecuperacionPasswordAsync(string email)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
                if (usuario is null)
                    return ResultDto<string>.FailureResult("Usuario no encontrado.");

                var token = Guid.NewGuid().ToString();
                var expiracion = DateTime.UtcNow.AddHours(1);

                usuario.GenerarTokenRecuperacion(token, expiracion);
                await _context.SaveChangesAsync();

                var asunto = "Recuperación de contraseña";
                var link = $"http://localhost:4200/recuperar-password?token={token}";
                var cuerpo = $@"
            <h2>Hola {usuario.Nombre}</h2>
            <p>Solicitaste restablecer tu contraseña. Haz clic en el siguiente enlace:</p>
            <a href='{link}'>Recuperar contraseña</a>
            <p>Este enlace expirará en 1 hora.</p>";

                await _icorreoService.EnviarCorreoAsync(email, asunto, cuerpo);

                return ResultDto<string>.SuccessResult("Token de recuperación enviado.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al enviar token: " + ex.Message);
            }
        }


        public async Task<ResultDto<string>> RecuperarPasswordAsync(RecuperarPasswordDto dto)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                    u.Email == dto.Email &&
                    u.TokenRecuperacion == dto.Token &&
                    u.ExpiraToken > DateTime.UtcNow);

                if (usuario is null)
                    return ResultDto<string>.FailureResult("Token inválido o expirado.");

                var hash = ComputeHash(dto.NuevaPassword);
                usuario.CambiarPassword(hash);
                usuario.LimpiarTokenRecuperacion();

                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Contraseña actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al recuperar contraseña." + ex.Message);
            }
        }

        public async Task<ResultDto<string>> ActivarUsuarioAsync(Guid id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                    return ResultDto<string>.FailureResult("Usuario no encontrado.");

                usuario.Activar();
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Usuario activado.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al activar usuario." + ex.Message);
            }
        }

        public async Task<ResultDto<string>> DesactivarUsuarioAsync(Guid id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                    return ResultDto<string>.FailureResult("Usuario no encontrado.");

                usuario.Desactivar();
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Usuario desactivado.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al desactivar usuario." + ex.Message);
            }
        }

        public async Task<ResultDto<string>> CambiarRolAsync(Guid usuarioId, string nuevoRol)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario == null)
                    return ResultDto<string>.FailureResult("Usuario no encontrado.");

                usuario.CambiarRol(nuevoRol);
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Rol actualizado.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al cambiar rol." + ex.Message);
            }
        }

        public async Task<ResultDto<string>> ActualizarAsync(Guid id, ActualizarUsuarioDto dto)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                    return ResultDto<string>.FailureResult("Usuario no encontrado.");

                usuario.Actualizar(dto.Nombre, dto.Email, dto.Rol);
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Usuario actualizado.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al actualizar usuario." + ex.Message);
            }
        }

        public async Task<ResultDto<IEnumerable<UsuarioDto>>> ObtenerTodosAsync()
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .Select(u => new UsuarioDto(u.Id, u.Nombre, u.Email, u.Rol, u.Activo, u.FechaRegistro))
                    .ToListAsync();

                return ResultDto<IEnumerable<UsuarioDto>>.SuccessResult(usuarios);
            }
            catch (Exception ex)
            {
                return ResultDto<IEnumerable<UsuarioDto>>.FailureResult("Error al obtener usuarios." + ex.Message);
            }
        }

        public async Task<ResultDto<UsuarioDto?>> ObtenerPorIdAsync(Guid id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                    return ResultDto<UsuarioDto?>.FailureResult("Usuario no encontrado.");

                var dto = new UsuarioDto(usuario.Id, usuario.Nombre, usuario.Email, usuario.Rol, usuario.Activo, usuario.FechaRegistro);
                return ResultDto<UsuarioDto?>.SuccessResult(dto);
            }
            catch (Exception ex)
            {
                return ResultDto<UsuarioDto?>.FailureResult("Error al obtener usuario." + ex.Message);
            }
        }

        public async Task<ResultDto<string>> EnviarVerificacionCuentaAsync(string email)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
                if (usuario is null)
                    return ResultDto<string>.FailureResult("Usuario no encontrado.");

                var token = Guid.NewGuid().ToString();
                var expiracion = DateTime.UtcNow.AddHours(24);

                usuario.GenerarTokenVerificacion(token, expiracion);
                await _context.SaveChangesAsync();

                var asunto = "Verifica tu cuenta";
                var cuerpo = $"Bienvenido/a. Verifica tu cuenta usando el siguiente token:\n\n{token}";

                await _icorreoService.EnviarCorreoAsync(email, asunto, cuerpo);

                return ResultDto<string>.SuccessResult("Correo de verificación enviado.");
            }
            catch (Exception ex)            {
                return ResultDto<string>.FailureResult("Error al enviar correo." + ex.Message);
            }
        }
        public async Task<ResultDto<string>> VerificarCuentaAsync(string token)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.TokenVerificacion == token);

                if (usuario is null || usuario.ExpiraVerificacion < DateTime.UtcNow)
                    return ResultDto<string>.FailureResult("Token inválido o expirado.");

                usuario.VerificarCuenta();
                await _context.SaveChangesAsync();

                return ResultDto<string>.SuccessResult("Cuenta verificada correctamente.");
            }
            catch (Exception ex)
            {
                return ResultDto<string>.FailureResult("Error al verificar cuenta." + ex.Message);
            }
        }

        private static string ComputeHash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
