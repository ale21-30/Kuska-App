using Kuska.Core.Entities;
using Kuska.Data;
using Microsoft.AspNetCore.Mvc;

namespace Kuska.Web.Controllers;

public class NinasController : Controller
{
    private readonly KuskaDbContext _context;
    private readonly AuditoriaService _auditoria;

    public NinasController(KuskaDbContext context, AuditoriaService auditoria)
    {
        _context = context;
        _auditoria = auditoria;
    }

    public IActionResult Crear()
    {
        if (HttpContext.Session.GetInt32("UsuarioId") == null)
            return RedirectToAction("Login", "Auth");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(string nombrePila, int edad, string? areaInteres)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null) return RedirectToAction("Login", "Auth");

        var nina = new Nina
        {
            MadreId = usuarioId.Value,
            NombrePila = nombrePila,
            Edad = edad,
            AreaInteres = areaInteres
        };

        _context.Ninas.Add(nina);
        await _context.SaveChangesAsync();

        await _auditoria.RegistrarAsync(
            accion: "REGISTRAR_NINA",
            entidad: "Nina",
            entidadId: nina.NinaId,
            usuarioId: usuarioId,
            descripcion: $"Niña registrada: {nombrePila}, {edad} años"
        );

        return RedirectToAction("Index", "Mentorias");
    }
}