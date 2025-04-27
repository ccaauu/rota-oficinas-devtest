using Xunit;
using Moq;
using System.Threading.Tasks;

public class ClientesControllerTests
{
    [Fact]
    public async Task GetById_DeveRetornarNotFound_SeClienteNaoExistir()
    {
        var mockRepo = new Mock<IClienteRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Cliente?)null);

        var controller = new ClientesController(mockRepo.Object);

        var result = await controller.GetById(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result);
    }
}
