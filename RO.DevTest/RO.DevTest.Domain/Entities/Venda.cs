public class Venda
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public DateTime Data { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; }
    public List<ItemVenda> Itens { get; set; } = new();
}
