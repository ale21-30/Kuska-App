using Kuska.Core.Entities;
using Kuska.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Kuska.Web.Controllers;

public class AuthController : Controller
{
    private readonly KuskaDbContext _context;
    private readonly AuditoriaService _auditoria;

    public AuthController(KuskaDbContext context, AuditoriaService auditoria)
    {
        _context = context;
        _auditoria = auditoria;
    }

    // ---- LOGIN ----
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var hash = HashPassword(password);
        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Email == email
                                   && u.PasswordHash == hash
                                   && u.Activo);

        if (usuario == null)
        {
            ViewBag.Error = "Email o contraseña incorrectos";
            return View();
        }

        // Guardar sesión
        HttpContext.Session.SetInt32("UsuarioId", usuario.UsuarioId);
        HttpContext.Session.SetString("Nombre", usuario.Nombre);
        HttpContext.Session.SetString("Rol", usuario.Rol.Nombre);

        await _auditoria.RegistrarAsync(
            accion: "LOGIN",
            entidad: "Usuario",
            entidadId: usuario.UsuarioId,
            usuarioId: usuario.UsuarioId,
            descripcion: $"Login exitoso — Rol: {usuario.Rol.Nombre}",
            ipAddress: HttpContext.Connection.RemoteIpAddress?.ToString()
        );

        // Redirigir según rol
        return usuario.Rol.Nombre switch
        {
            "Admin" => RedirectToAction("Index", "Dashboard"),
            "Madre" => RedirectToAction("Index", "Fondos"),
            "Mentora" => RedirectToAction("Index", "Mentorias"),
            "Empresa" => RedirectToAction("Index", "Empresa"),
            _ => RedirectToAction("Index", "Home")
        };
    }

    // ---- REGISTRO ----
    public async Task<IActionResult> Registro()
    {
        ViewBag.Roles = await _context.Roles
            .Where(r => r.Nombre != "Admin")
            .ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registro(string nombre, string email,
                                               string password, int rolId,
                                               string? cedula)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == email))
        {
            ViewBag.Error = "Ya existe una cuenta con ese email";
            ViewBag.Roles = await _context.Roles
                .Where(r => r.Nombre != "Admin").ToListAsync();
            return View();
        }

        var usuario = new Usuario
        {
            Nombre = nombre,
            Email = email,
            PasswordHash = HashPassword(password),
            RolId = rolId,
            Cedula = cedula,
            // Mentoras requieren verificación manual
            CedulaVerificada = false
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        await _auditoria.RegistrarAsync(
            accion: "REGISTRO",
            entidad: "Usuario",
            entidadId: usuario.UsuarioId,
            descripcion: $"Nuevo registro — RolId: {rolId}"
        );

        return RedirectToAction("Login");
    }

    // ---- LOGOUT ----
    public async Task<IActionResult> Logout()
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        await _auditoria.RegistrarAsync(
            accion: "LOGOUT",
            usuarioId: usuarioId,
            descripcion: "Cierre de sesión"
        );
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    // ---- HELPER ----
    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}