public class ClienteDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string CPF { get; set; } = null!;
    public DateTime DataNascimento { get; set; }
}
