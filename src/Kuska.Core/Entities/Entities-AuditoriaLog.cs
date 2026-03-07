namespace Kuska.Core.Entities;

public class AuditoriaLog
{
    public int LogId { get; set; }
    public int? UsuarioId { get; set; }
    public string Accion { get; set; } = string.Empty;
    public string? Entidad { get; set; }
    public int? EntidadId { get; set; }
    public string? Descripcion { get; set; }
    public string? IpAddress { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.Now;

    // Navegación
    public Usuario? Usuario { get; set; }
}