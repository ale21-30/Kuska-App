namespace Kuska.Core.Entities;

public class Reporte
{
    public int ReporteId { get; set; }
    public int SesionId { get; set; }
    public int ReportadoPor { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = "Pendiente";
    public DateTime FechaReporte { get; set; } = DateTime.Now;
    public DateTime? FechaResolucion { get; set; }

    // Navegación
    public Sesion Sesion { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}