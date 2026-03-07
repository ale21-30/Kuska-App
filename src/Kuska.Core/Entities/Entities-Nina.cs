using static System.Collections.Specialized.BitVector32;

namespace Kuska.Core.Entities;

public class Nina
{
    public int NinaId { get; set; }
    public int MadreId { get; set; }
    public string NombrePila { get; set; } = string.Empty; // Solo nombre de pila
    public int Edad { get; set; }
    public string? AreaInteres { get; set; }
    public bool Activa { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    // Navegación
    public Usuario Madre { get; set; } = null!;
    public ICollection<Sesion> Sesiones { get; set; } = new List<Sesion>();
}