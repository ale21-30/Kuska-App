using static System.Collections.Specialized.BitVector32;
using Kuska.Core.Entities;

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

    public string? Historia { get; set; }
    public ICollection<ListaUtil> ListaUtiles { get; set; } = new List<ListaUtil>();
    public ICollection<Apadrinamiento> Apadrinamientos { get; set; } = new List<Apadrinamiento>();

    // Navegación
    public Usuario Madre { get; set; } = null!;
    public ICollection<Sesion> Sesiones { get; set; } = new List<Sesion>();
}