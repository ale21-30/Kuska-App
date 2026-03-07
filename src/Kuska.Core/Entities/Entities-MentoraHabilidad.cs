namespace Kuska.Core.Entities;

public class MentoraHabilidad
{
    public int HabilidadId { get; set; }
    public int MentoraId { get; set; }
    public string Habilidad { get; set; } = string.Empty;

    // Navegación
    public Mentora Mentora { get; set; } = null!;
}