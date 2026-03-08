namespace Kuska.Core.Entities;

public class ListaUtil
{
    public int ItemId { get; set; }
    public int NinaId { get; set; }
    public string Descripcion { get; set; } = "";
    public int Cantidad { get; set; } = 1;
    public decimal PrecioEstimado { get; set; }
    public bool Cubierto { get; set; }
    public int? ApadrinadoPor { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaCubierto { get; set; }

    public Nina Nina { get; set; } = null!;
    public Usuario? Apadrinador { get; set; }
}