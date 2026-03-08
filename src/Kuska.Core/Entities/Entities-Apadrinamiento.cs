namespace Kuska.Core.Entities;

public class Apadrinamiento
{
    public int ApadrinamientoId { get; set; }
    public int NinaId { get; set; }
    public int EmpresaId { get; set; }
    public decimal MontoTotal { get; set; }
    public string Estado { get; set; } = "Activo";
    public DateTime FechaInicio { get; set; }
    public string? Observacion { get; set; }

    public Nina Nina { get; set; } = null!;
    public Usuario Empresa { get; set; } = null!;
}