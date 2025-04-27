public interface IUserRepository
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task AddAsync(Usuario user);
}
