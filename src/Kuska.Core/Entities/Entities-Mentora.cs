using static System.Collections.Specialized.BitVector32;

namespace Kuska.Core.Entities;

public class Mentora
{
    public int MentoraId { get; set; }
    public int UsuarioId { get; set; }
    public string Especialidad { get; set; } = string.Empty;
    public string? Ciudad { get; set; }
    public string? Bio { get; set; }
    public decimal Rating { get; set; }
    public int TotalSesiones { get; set; }
    public bool Verificada { get; set; }
    public DateTime? FechaVerificacion { get; set; }

    // Navegación
    public Usuario Usuario { get; set; } = null!;
    public ICollection<MentoraHabilidad> Habilidades { get; set; } = new List<MentoraHabilidad>();
    public ICollection<Sesion> Sesiones { get; set; } = new List<Sesion>();
}