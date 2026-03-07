namespace Kuska.Core.Entities;

public class Fondo
{
    public int FondoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Escuela { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public decimal MetaMonto { get; set; }
    public decimal MontoActual { get; set; }
    public DateTime FechaInicio { get; set; } = DateTime.Now;
    public DateTime FechaFin { get; set; }
    public string Estado { get; set; } = "Activo";
    public int? CreadoPor { get; set; }

    // Navegación
    public ICollection<AporteFondo> Aportes { get; set; } = new List<AporteFondo>();

    // Propiedad calculada
    public decimal PorcentajeAvance => MetaMonto > 0
        ? Math.Round((MontoActual / MetaMonto) * 100, 1)
        : 0;
}