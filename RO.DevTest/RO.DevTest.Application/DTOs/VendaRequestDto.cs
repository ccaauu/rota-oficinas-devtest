public class VendaRequestDto
{
    public Guid ClienteId { get; set; }
    public List<ItemVendaDto> Itens { get; set; } = new();
}