public interface IVendaRepository
{
    Task<IEnumerable<Venda>> GetAllAsync();
    Task<Venda?> GetByIdAsync(Guid id);
    Task AddAsync(Venda venda);
}
