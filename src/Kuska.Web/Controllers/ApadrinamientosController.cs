using Kuska.Core.Entities;
using Kuska.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kuska.Web.Controllers;

public class ApadrinamientosController : Controller
{
    private readonly KuskaDbContext _context;

    public ApadrinamientosController(KuskaDbContext context)
    {
        _context = context;
    }

    // Galería pública de niñas para apadrinar
    public async Task<IActionResult> Index()
    {
        var ninas = await _context.Ninas
            .Include(n => n.ListaUtiles)
            .Include(n => n.Apadrinamientos)
            .Where(n => n.Activa)
            .ToListAsync();
        return View(ninas);
    }

    // Lista de útiles de una niña específica
    public async Task<IActionResult> Perfil(int ninaId)
    {
        var nina = await _context.Ninas
            .Include(n => n.ListaUtiles)
            .Include(n => n.Apadrinamientos)
                .ThenInclude(a => a.Empresa)
            .FirstOrDefaultAsync(n => n.NinaId == ninaId);

        if (nina == null) return NotFound();
        return View(nina);
    }

    // Apadrinar desde el perfil
    [HttpPost]
    public async Task<IActionResult> Apadrinar(int ninaId, List<int> itemIds)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId == null) return RedirectToAction("Login", "Auth");

        var items = await _context.Set<ListaUtil>()
            .Where(i => itemIds.Contains(i.ItemId) && !i.Cubierto)
            .ToListAsync();

        decimal total = 0;
        foreach (var item in items)
        {
            item.Cubierto = true;
            item.ApadrinadoPor = usuarioId;
            item.FechaCubierto = DateTime.Now;
            total += item.PrecioEstimado * item.Cantidad;
        }

        if (items.Any())
        {
            var apadrinamiento = new Apadrinamiento
            {
                NinaId = ninaId,
                EmpresaId = usuarioId.Value,
                MontoTotal = total,
                Estado = "Activo",
                FechaInicio = DateTime.Now
            };
            _context.Set<Apadrinamiento>().Add(apadrinamiento);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Perfil", new { ninaId });
    }
}