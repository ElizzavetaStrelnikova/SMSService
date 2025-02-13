using Moq;
using SMSService.Interfaces;
using SMSService.Models;
using Xunit;

public class MessageRepositoryTests
{
    private readonly Mock<IMessageRepository> _mockRepository;

    public MessageRepositoryTests()
    {
        _mockRepository = new Mock<IMessageRepository>();
    }

    [Fact]
    public async Task GetMessagesAsync_Test()
    {
        // Arrange
        var expectedMessages = new List<Message>
        {
            new Message { Id = 1, Text = "Hello!" },
            new Message { Id = 2, Text = "World!" }
        };
        _mockRepository.Setup(repo => repo.GetMessagesAsync()).ReturnsAsync(expectedMessages);

        // Act
        var result = await _mockRepository.Object.GetMessagesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetMessageByIdAsync_Test()
    {
        // Arrange
        var expectedMessage = new Message { Id = 1, Text = "Hello!" };
        _mockRepository.Setup(repo => repo.GetMessageByIdAsync(1)).ReturnsAsync(expectedMessage);

        // Act
        var result = await _mockRepository.Object.GetMessageByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Hello!", result.Text);
    }

    [Fact]
    public async Task AddMessageAsync_Test()
    {
        // Arrange
        var newMessage = new Message { Id = 3, Text = "New Message" };
        _mockRepository.Setup(repo => repo.AddMessageAsync(newMessage)).Returns(Task.CompletedTask);

        // Act
        await _mockRepository.Object.AddMessageAsync(newMessage);

        // Assert
        _mockRepository.Verify(r => r.AddMessageAsync(newMessage), Times.Once);
    }
}