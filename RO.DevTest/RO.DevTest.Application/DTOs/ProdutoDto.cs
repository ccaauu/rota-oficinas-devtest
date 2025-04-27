public class ProdutoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}
