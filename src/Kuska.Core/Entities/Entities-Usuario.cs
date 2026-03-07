namespace Kuska.Core.Entities;

public class Usuario
{
    public int UsuarioId { get; set; }
    public int RolId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Cedula { get; set; }
    public bool CedulaVerificada { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    // Navegación
    public Rol Rol { get; set; } = null!;
    public ICollection<Nina> Ninas { get; set; } = new List<Nina>();
    public Mentora? Mentora { get; set; }
}