using Microsoft.AspNetCore.Mvc;
using SMSService.Interfaces;
using SMSService.Models;

namespace SMSService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMessageRepository _messageRepository;

        public MessageController(ILogger<MessageController> logger, IMessageRepository messageRepository)
        {
            _logger = logger;
            _messageRepository = messageRepository;
        }

        /// <summary>
        /// Get the list of messages for the definite date-time period.
        /// </summary>
        /// <param name="CreatedAt">The date-time period for the list of messages.</param>
        /// <returns>the list of messages</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            var messages = await _messageRepository.GetMessagesAsync();
            return Ok(messages);
        }

        /// <summary>
        /// Save the list of messages to the database.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> PostMessage([FromBody] Message message)
        {
            message.CreatedAt = DateTime.UtcNow;
            await _messageRepository.AddMessageAsync(message);
            return CreatedAtAction(nameof(GetMessages), new { id = message.Id }, message);
        }

        /// <summary>
        /// Send the list of messages to the client.
        /// </summary>
        /// /// <param name="Id">The identifier of the message.</param>
        /// <returns>the list of messages</returns>
        [HttpPost]
        public async Task<ActionResult> SendMessage([FromBody] Message message)
        {
            message.CreatedAt = DateTime.UtcNow;
            await _messageRepository.AddMessageAsync(message);
            return CreatedAtAction(nameof(GetMessages), new { id = message.Id }, message);
        }
    }
}
