using Xunit;
using Moq;
using System.Threading.Tasks;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_DeveRegistrarUsuarioERetornarToken()
    {
        var mockRepo = new Mock<IUserRepository>();
        var mockToken = new Mock<ITokenService>();

        mockRepo.Setup(r => r.GetByEmailAsync("novo@teste.com"))
                .ReturnsAsync((Usuario?)null);

        mockToken.Setup(t => t.GenerateToken(It.IsAny<Usuario>()))
                 .Returns("fake-jwt-token");

        var authService = new AuthService(mockRepo.Object, mockToken.Object);

        var request = new RegisterRequest
        {
            Nome = "Teste",
            Email = "novo@teste.com",
            Senha = "Senha123",
            Cargo = "Admin"
        };

        var result = await authService.RegisterAsync(request);

        Assert.Equal("novo@teste.com", result.Email);
        Assert.Equal("Admin", result.Cargo);
        Assert.Equal("fake-jwt-token", result.Token);
        mockRepo.Verify(r => r.AddAsync(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_DeveRetornarToken_QuandoCredenciaisForemValidas()
    {
        var mockRepo = new Mock<IUserRepository>();
        var mockToken = new Mock<ITokenService>();

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "Usuário Teste",
            Email = "user@teste.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("Senha123"),
            Cargo = "Admin"
        };

        mockRepo.Setup(r => r.GetByEmailAsync(usuario.Email)).ReturnsAsync(usuario);
        mockToken.Setup(t => t.GenerateToken(It.IsAny<Usuario>())).Returns("fake-token");

        var service = new AuthService(mockRepo.Object, mockToken.Object);

        var login = new LoginRequest { Email = usuario.Email, Senha = "Senha123" };

        var result = await service.LoginAsync(login);

        Assert.Equal(usuario.Email, result.Email);
        Assert.Equal(usuario.Cargo, result.Cargo);
        Assert.Equal("fake-token", result.Token);
    }
}
