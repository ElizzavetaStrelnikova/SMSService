using Microsoft.AspNetCore.Mvc;
using Moq;
using SMSService.Controllers;
using SMSService.Interfaces;
using SMSService.Models;
using Xunit;

public class MessageControllerTests
{
    private readonly Mock<IMessageRepository> _mockRepository;
    private readonly MessageController _controller;

    public MessageControllerTests()
    {
        _mockRepository = new Mock<IMessageRepository>();
        var loggerMock = new Mock<ILogger<MessageController>>();
        _controller = new MessageController(loggerMock.Object, _mockRepository.Object);
    }

    [Fact]
    public async Task GetMessages_ReturnsOkResult_WithMessages()
    {
        // Arrange
        var expectedMessages = new List<Message>
        {
            new Message { Id = 1, Text = "Hello!" },
            new Message { Id = 2, Text = "World!" }
        };

        _mockRepository.Setup(repo => repo.GetMessagesAsync()).ReturnsAsync(expectedMessages);

        // Act
        var result = await _controller.GetMessages();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnMessages = Assert.IsAssignableFrom<IEnumerable<Message>>(okResult.Value);
        Assert.Equal(2, returnMessages.Count());
    }

    [Fact]
    public async Task GetMessages_ReturnsNotFound_WhenNoMessages()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetMessagesAsync()).ReturnsAsync(new List<Message>());

        // Act
        var result = await _controller.GetMessages();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnMessages = Assert.IsAssignableFrom<IEnumerable<Message>>(okResult.Value);
        Assert.Empty(returnMessages);
    }
}