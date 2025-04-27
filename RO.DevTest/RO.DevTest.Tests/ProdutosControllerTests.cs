using Xunit;
using Moq;
using System.Threading.Tasks;

public class ProdutosControllerTests
{
    [Fact]
    public async Task Create_DeveRetornarCreated_SeProdutoForValido()
    {
        var mockRepo = new Mock<IProdutoRepository>();
        var controller = new ProdutosController(mockRepo.Object);

        var produtoDto = new ProdutoDto
        {
            Nome = "Notebook",
            Preco = 2500m,
            Estoque = 10
        };

        var result = await controller.Create(produtoDto);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        var produto = Assert.IsType<Produto>(created.Value);
        Assert.Equal(produtoDto.Nome, produto.Nome);
        Assert.Equal(produtoDto.Preco, produto.Preco);
    }
}
