using Xunit;
using System.Linq;

public class PaginacaoTests
{
    [Fact]
    public async Task Paginacao_DeveRetornarPaginaCorreta()
    {
        var lista = Enumerable.Range(1, 20).Select(x => new Produto { Id = Guid.NewGuid(), Nome = $"Produto {x}", Preco = 10, Estoque = 10 }).AsQueryable();

        var resultado = await lista.PaginarAsync(2, 5);

        Assert.Equal(2, resultado.PaginaAtual);
        Assert.Equal(5, resultado.Dados.Count);
        Assert.Equal(20, resultado.TotalRegistros);
    }
}
