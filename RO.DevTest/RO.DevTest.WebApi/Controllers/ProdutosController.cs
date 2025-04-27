[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutosController(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    [HttpGet("filtrado")]
    public async Task<IActionResult> GetProdutosFiltrado([FromQuery] PaginacaoFiltro filtro)
    {
        var query = _produtoRepository.QueryAll();
        if (!string.IsNullOrEmpty(filtro.TermoBusca))
            query = query.Where(p => p.Nome.Contains(filtro.TermoBusca));

        query = query.AplicarOrdenacao(filtro.OrdenarPor, filtro.OrdemAscendente);
        var resultado = await query.PaginarAsync(filtro.Pagina, filtro.TamanhoPagina);
        return Ok(resultado);
    }
}
