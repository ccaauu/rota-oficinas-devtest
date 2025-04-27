[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ClientesController : ControllerBase
{
    private readonly IClienteRepository _clienteRepository;

    public ClientesController(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    [HttpGet("filtrado")]
    public async Task<IActionResult> GetClientesFiltrado([FromQuery] PaginacaoFiltro filtro)
    {
        var query = _clienteRepository.QueryAll();
        if (!string.IsNullOrEmpty(filtro.TermoBusca))
            query = query.Where(c => c.Nome.Contains(filtro.TermoBusca));

        query = query.AplicarOrdenacao(filtro.OrdenarPor, filtro.OrdemAscendente);
        var resultado = await query.PaginarAsync(filtro.Pagina, filtro.TamanhoPagina);
        return Ok(resultado);
    }
}
