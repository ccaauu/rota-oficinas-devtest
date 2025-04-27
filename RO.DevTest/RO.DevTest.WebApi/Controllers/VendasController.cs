[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class VendasController : ControllerBase
{
    private readonly IVendaRepository _vendaRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IClienteRepository _clienteRepository;

    public VendasController(IVendaRepository vendaRepository, IProdutoRepository produtoRepository, IClienteRepository clienteRepository)
    {
        _vendaRepository = vendaRepository;
        _produtoRepository = produtoRepository;
        _clienteRepository = clienteRepository;
    }

    [HttpGet("filtrado")]
    public async Task<IActionResult> GetVendasFiltrado([FromQuery] PaginacaoFiltro filtro)
    {
        var vendas = await _vendaRepository.GetAllAsync();
        var query = vendas.AsQueryable();

        query = query.AplicarOrdenacao(filtro.OrdenarPor, filtro.OrdemAscendente);
        var resultado = await query.PaginarAsync(filtro.Pagina, filtro.TamanhoPagina);
        return Ok(resultado);
    }

    [HttpPost("relatorio")]
    public async Task<IActionResult> Relatorio([FromBody] VendaRelatorioRequest request)
    {
        var vendas = await _vendaRepository.GetAllAsync();
        var filtradas = vendas.Where(v => v.Data >= request.Inicio && v.Data <= request.Fim);

        var totalVendas = filtradas.Count();
        var rendaTotal = filtradas.Sum(v => v.Total);

        var produtos = filtradas
            .SelectMany(v => v.Itens)
            .GroupBy(i => i.Produto.Nome)
            .Select(g => new ProdutoRelatorioDto
            {
                NomeProduto = g.Key,
                QuantidadeVendida = g.Sum(i => i.Quantidade),
                RendaGerada = g.Sum(i => i.PrecoUnitario * i.Quantidade)
            })
            .ToList();

        var resultado = new VendaRelatorioResponse
        {
            TotalVendas = totalVendas,
            RendaTotal = rendaTotal,
            ProdutosVendidos = produtos
        };

        return Ok(resultado);
    }
}
