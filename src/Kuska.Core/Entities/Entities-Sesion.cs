namespace Kuska.Core.Entities;

public class Sesion
{
    public int SesionId { get; set; }
    public int MentoraId { get; set; }
    public int NinaId { get; set; }
    public int SupervisoraId { get; set; }
    public DateTime FechaSesion { get; set; }
    public int DuracionMinutos { get; set; } = 60;
    public string Estado { get; set; } = "Pendiente";
    public string? LinkSala { get; set; }
    public DateTime? LinkExpiracion { get; set; }
    public string? Tema { get; set; }

    // Post-sesión
    public bool? AprobadaPorTutor { get; set; }
    public int? RatingMentora { get; set; }
    public string? Comentario { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    // Navegación
    public Mentora Mentora { get; set; } = null!;
    public Nina Nina { get; set; } = null!;
    public Usuario Supervisora { get; set; } = null!;
    public ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();
}