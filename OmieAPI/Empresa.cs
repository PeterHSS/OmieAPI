namespace OmieAPI;

public class Empresa
{
    public long Codigo { get; set; }
    public required string Nome { get; set; }
    public required string AppChave { get; set; }
    public required string AppSegredo { get; set; }
    public DateTime Criacao { get; }


    public override string ToString()
    {
        return $"Codigo {Codigo}, Nome {Nome}, AppChave {AppChave}, AppSegredo {AppSegredo}, Criacao {Criacao} \n";
    }
}
