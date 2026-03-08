using Kuska.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Kuska.Web.Controllers;

public class SesionesController : Controller
{
    private readonly KuskaDbContext _context;
    private readonly AuditoriaService _auditoria;
    private readonly IConfiguration _config;

    public SesionesController(KuskaDbContext context,
                               AuditoriaService auditoria,
                               IConfiguration config)
    {
        _context = context;
        _auditoria = auditoria;
        _config = config;
    }

    // ---- ENTRAR A SALA ----
    public async Task<IActionResult> Sala(int sesionId)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null) return RedirectToAction("Login", "Auth");

        var sesion = await _context.Sesiones
            .Include(s => s.Mentora).ThenInclude(m => m.Usuario)
            .Include(s => s.Nina)
            .Include(s => s.Supervisora)
            .FirstOrDefaultAsync(s => s.SesionId == sesionId);

        if (sesion == null) return NotFound();

        // Generar link si no existe o expiró
        if (sesion.LinkSala == null || sesion.LinkExpiracion < DateTime.Now)
        {
            var link = await GenerarSalaAsync(sesionId);
            sesion.LinkSala = link;
            sesion.LinkExpiracion = DateTime.Now.AddMinutes(90);
            sesion.Estado = "EnCurso";
            await _context.SaveChangesAsync();
        }

        await _auditoria.RegistrarAsync(
            accion: "ENTRAR_SALA",
            entidad: "Sesion",
            entidadId: sesionId,
            usuarioId: usuarioId,
            descripcion: "Usuario entró a la sala de videollamada",
            ipAddress: HttpContext.Connection.RemoteIpAddress?.ToString()
        );

        return View(sesion);
    }

    // ---- REPORTAR EN SALA ----
    [HttpPost]
    public async Task<IActionResult> Reportar(int sesionId, string descripcion)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null) return RedirectToAction("Login", "Auth");

        var reporte = new Kuska.Core.Entities.Reporte
        {
            SesionId = sesionId,
            ReportadoPor = usuarioId.Value,
            Descripcion = descripcion,
            Estado = "Pendiente"
        };

        // Suspender sesión inmediatamente
        var sesion = await _context.Sesiones.FindAsync(sesionId);
        if (sesion != null)
        {
            sesion.Estado = "Cancelada";
            sesion.LinkSala = null;
        }

        _context.Reportes.Add(reporte);
        await _context.SaveChangesAsync();

        await _auditoria.RegistrarAsync(
            accion: "REPORTAR_SESION",
            entidad: "Sesion",
            entidadId: sesionId,
            usuarioId: usuarioId,
            descripcion: $"⚠️ REPORTE: {descripcion}"
        );

        return RedirectToAction("Index", "Home");
    }

    // ---- GENERAR SALA DAILY.CO ----
    private async Task<string> GenerarSalaAsync(int sesionId)
    {
        var apiKey = _config["Daily:ApiKey"];

        // Si no hay API key configurada, devuelve link de demo
        if (string.IsNullOrEmpty(apiKey))
            return $"https://demo.daily.co/sesion-{sesionId}";

        using var http = new HttpClient();
        http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var body = new
        {
            name = $"kuska-sesion-{sesionId}-{Guid.NewGuid():N}",
            properties = new
            {
                exp = DateTimeOffset.Now.AddMinutes(90).ToUnixTimeSeconds(),
                max_participants = 3,        // Máximo 3: niña, mentora, supervisora
                enable_recording = false
            }
        };

        var response = await http.PostAsJsonAsync(
            "https://api.daily.co/v1/rooms", body);

        if (!response.IsSuccessStatusCode)
            return $"https://demo.daily.co/sesion-{sesionId}";

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);
        var roomUrl = doc.RootElement.GetProperty("url").GetString();

        return roomUrl ?? $"https://demo.daily.co/sesion-{sesionId}";
    }
}