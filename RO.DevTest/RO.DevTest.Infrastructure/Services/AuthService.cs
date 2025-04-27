public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new Exception("Usuário já cadastrado.");

        var usuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
            Cargo = request.Cargo
        };

        await _userRepository.AddAsync(usuario);

        var token = _tokenService.GenerateToken(usuario);

        return new AuthResponse
        {
            Email = usuario.Email,
            Cargo = usuario.Cargo,
            Token = token
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Senha, user.SenhaHash))
            throw new Exception("Usuário ou senha inválidos.");

        var token = _tokenService.GenerateToken(user);

        return new AuthResponse
        {
            Email = user.Email,
            Cargo = user.Cargo,
            Token = token
        };
    }
}
