using Kuska.Core.Entities;
using Kuska.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kuska.Web.Controllers;

public class MentoriasController : Controller
{
    private readonly KuskaDbContext _context;
    private readonly AuditoriaService _auditoria;

    public MentoriasController(KuskaDbContext context, AuditoriaService auditoria)
    {
        _context = context;
        _auditoria = auditoria;
    }

    // ---- LISTA MENTORAS ----
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetInt32("UsuarioId") == null)
            return RedirectToAction("Login", "Auth");

        var mentoras = await _context.Mentoras
            .Include(m => m.Usuario)
            .Include(m => m.Habilidades)
            .Where(m => m.Verificada)
            .OrderByDescending(m => m.Rating)
            .ToListAsync();

        return View(mentoras);
    }

    // ---- AGENDAR SESIÓN ----
    public async Task<IActionResult> Agendar(int mentoraId)
    {
        if (HttpContext.Session.GetInt32("UsuarioId") == null)
            return RedirectToAction("Login", "Auth");

        var mentora = await _context.Mentoras
            .Include(m => m.Usuario)
            .Include(m => m.Habilidades)
            .FirstOrDefaultAsync(m => m.MentoraId == mentoraId);

        if (mentora == null) return NotFound();

        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        ViewBag.Ninas = await _context.Ninas
            .Where(n => n.MadreId == usuarioId && n.Activa)
            .ToListAsync();

        ViewBag.Mentora = mentora;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Agendar(int mentoraId, int ninaId,
                                             DateTime fechaSesion, string tema)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null) return RedirectToAction("Login", "Auth");

        var sesion = new Sesion
        {
            MentoraId = mentoraId,
            NinaId = ninaId,
            SupervisoraId = usuarioId.Value,
            FechaSesion = fechaSesion,
            Tema = tema,
            Estado = "Pendiente"
        };

        _context.Sesiones.Add(sesion);
        await _context.SaveChangesAsync();

        await _auditoria.RegistrarAsync(
            accion: "AGENDAR_SESION",
            entidad: "Sesion",
            entidadId: sesion.SesionId,
            usuarioId: usuarioId,
            descripcion: $"Sesión agendada — Mentora: {mentoraId}, Niña: {ninaId}"
        );

        return RedirectToAction("MisSesiones");
    }

    // ---- MIS SESIONES ----
    public async Task<IActionResult> MisSesiones()
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null) return RedirectToAction("Login", "Auth");

        var sesiones = await _context.Sesiones
            .Include(s => s.Mentora).ThenInclude(m => m.Usuario)
            .Include(s => s.Nina)
            .Where(s => s.SupervisoraId == usuarioId)
            .OrderByDescending(s => s.FechaSesion)
            .ToListAsync();

        return View(sesiones);
    }

    // ---- EVALUAR SESIÓN ----
    [HttpPost]
    public async Task<IActionResult> Evaluar(int sesionId, bool aprobada,
                                             int rating, string? comentario)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null) return RedirectToAction("Login", "Auth");

        var sesion = await _context.Sesiones
            .Include(s => s.Mentora)
            .FirstOrDefaultAsync(s => s.SesionId == sesionId
                                   && s.SupervisoraId == usuarioId);

        if (sesion == null) return NotFound();

        sesion.AprobadaPorTutor = aprobada;
        sesion.RatingMentora = rating;
        sesion.Comentario = comentario;
        sesion.Estado = "Completada";

        // Actualizar rating de mentora
        var mentora = sesion.Mentora;
        mentora.TotalSesiones++;
        var sesionesConRating = await _context.Sesiones
            .Where(s => s.MentoraId == mentora.MentoraId
                     && s.RatingMentora != null)
            .ToListAsync();

        if (sesionesConRating.Any())
            mentora.Rating = (decimal)sesionesConRating
                .Average(s => s.RatingMentora!.Value);

        await _context.SaveChangesAsync();

        await _auditoria.RegistrarAsync(
            accion: "EVALUAR_SESION",
            entidad: "Sesion",
            entidadId: sesionId,
            usuarioId: usuarioId,
            descripcion: $"Evaluación: Aprobada={aprobada}, Rating={rating}"
        );

        return RedirectToAction("MisSesiones");
    }
}