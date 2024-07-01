namespace OmieAPI.Entidades;

public class Empresa
{
    public long Codigo { get; set; }
    public required string Nome { get; set; }
    public required string AppChave { get; set; }
    public required string AppSegredo { get; set; }
    public DateTime Criacao { get; }
}
