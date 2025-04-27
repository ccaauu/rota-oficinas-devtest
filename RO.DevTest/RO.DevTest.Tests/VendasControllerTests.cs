using Xunit;
using Moq;
using System.Threading.Tasks;

public class VendasControllerTests
{
    [Fact]
    public async Task Create_DeveRetornarBadRequest_SeEstoqueInsuficiente()
    {
        var mockVendaRepo = new Mock<IVendaRepository>();
        var mockProdRepo = new Mock<IProdutoRepository>();
        var mockCliRepo = new Mock<IClienteRepository>();

        var cliente = new Cliente { Id = Guid.NewGuid(), Nome = "Teste", Email = "c@x.com", CPF = "123", DataNascimento = DateTime.Today };
        var produto = new Produto { Id = Guid.NewGuid(), Nome = "Produto", Estoque = 1, Preco = 10 };

        mockCliRepo.Setup(r => r.GetByIdAsync(cliente.Id)).ReturnsAsync(cliente);
        mockProdRepo.Setup(r => r.GetByIdAsync(produto.Id)).ReturnsAsync(produto);

        var controller = new VendasController(mockVendaRepo.Object, mockProdRepo.Object, mockCliRepo.Object);

        var request = new VendaRequestDto
        {
            ClienteId = cliente.Id,
            Itens = new List<ItemVendaDto>
            {
                new ItemVendaDto { ProdutoId = produto.Id, Quantidade = 5 }
            }
        };

        var result = await controller.Create(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("estoque insuficiente", badRequest.Value!.ToString(), StringComparison.OrdinalIgnoreCase);
    }
}
