using Kuska.Core.Entities;

namespace Kuska.Data;

public class AuditoriaService
{
    private readonly KuskaDbContext _context;

    public AuditoriaService(KuskaDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarAsync(
        string accion,
        string? entidad = null,
        int? entidadId = null,
        int? usuarioId = null,
        string? descripcion = null,
        string? ipAddress = null)
    {
        var log = new AuditoriaLog
        {
            Accion = accion,
            Entidad = entidad,
            EntidadId = entidadId,
            UsuarioId = usuarioId,
            Descripcion = descripcion,
            IpAddress = ipAddress,
            FechaHora = DateTime.Now
        };

        _context.AuditoriaLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}