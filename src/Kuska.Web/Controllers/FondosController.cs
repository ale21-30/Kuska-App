using Kuska.Core.Entities;
using Kuska.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kuska.Web.Controllers;

public class FondosController : Controller
{
    private readonly KuskaDbContext _context;
    private readonly AuditoriaService _auditoria;

    public FondosController(KuskaDbContext context, AuditoriaService auditoria)
    {
        _context = context;
        _auditoria = auditoria;
    }

    // ---- LISTA DE FONDOS ----
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetInt32("UsuarioId") == null)
            return RedirectToAction("Login", "Auth");

        var fondos = await _context.Fondos
            .Include(f => f.Aportes)
            .Where(f => f.Estado == "Activo")
            .OrderByDescending(f => f.FechaInicio)
            .ToListAsync();

        return View(fondos);
    }

    // ---- DETALLE ----
    public async Task<IActionResult> Detalle(int id)
    {
        if (HttpContext.Session.GetInt32("UsuarioId") == null)
            return RedirectToAction("Login", "Auth");

        var fondo = await _context.Fondos
            .Include(f => f.Aportes)
            .ThenInclude(a => a.Usuario)
            .FirstOrDefaultAsync(f => f.FondoId == id);

        if (fondo == null) return NotFound();
        return View(fondo);
    }

    // ---- CREAR FONDO ----
    public IActionResult Crear()
    {
        if (HttpContext.Session.GetInt32("UsuarioId") == null)
            return RedirectToAction("Login", "Auth");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(string nombre, string escuela,
                                           string ciudad, decimal metaMonto,
                                           DateTime fechaFin)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null) return RedirectToAction("Login", "Auth");

        var fondo = new Fondo
        {
            Nombre = nombre,
            Escuela = escuela,
            Ciudad = ciudad,
            MetaMonto = metaMonto,
            FechaFin = fechaFin,
            CreadoPor = usuarioId
        };

        _context.Fondos.Add(fondo);
        await _context.SaveChangesAsync();

        await _auditoria.RegistrarAsync(
            accion: "CREAR_FONDO",
            entidad: "Fondo",
            entidadId: fondo.FondoId,
            usuarioId: usuarioId,
            descripcion: $"Fondo creado: {nombre} — Meta: ${metaMonto}"
        );

        return RedirectToAction("Index");
    }

    // ---- APORTAR ----
    [HttpPost]
    public async Task<IActionResult> Aportar(int fondoId, decimal monto,
                                             string fuente, string? empresa)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null) return RedirectToAction("Login", "Auth");

        var fondo = await _context.Fondos.FindAsync(fondoId);
        if (fondo == null) return NotFound();

        var aporte = new AporteFondo
        {
            FondoId = fondoId,
            UsuarioId = usuarioId.Value,
            Monto = monto,
            Fuente = fuente,
            Empresa = empresa
        };

        fondo.MontoActual += monto;

        if (fondo.MontoActual >= fondo.MetaMonto)
            fondo.Estado = "Completado";

        _context.AportesFondo.Add(aporte);
        await _context.SaveChangesAsync();

        await _auditoria.RegistrarAsync(
            accion: "APORTAR_FONDO",
            entidad: "Fondo",
            entidadId: fondoId,
            usuarioId: usuarioId,
            descripcion: $"Aporte de ${monto} — Fuente: {fuente}"
        );

        return RedirectToAction("Detalle", new { id = fondoId });
    }
}