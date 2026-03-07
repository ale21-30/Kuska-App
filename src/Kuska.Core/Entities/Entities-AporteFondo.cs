namespace Kuska.Core.Entities;

public class AporteFondo
{
    public int AporteId { get; set; }
    public int FondoId { get; set; }
    public int UsuarioId { get; set; }
    public decimal Monto { get; set; }
    public string Fuente { get; set; } = string.Empty; // Madre, RSE, Cooperacion
    public string? Empresa { get; set; }
    public DateTime FechaAporte { get; set; } = DateTime.Now;
    public string? Descripcion { get; set; }

    // Navegación
    public Fondo Fondo { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}