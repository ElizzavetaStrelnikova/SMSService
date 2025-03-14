using Newtonsoft.Json;
using SMSService.Controllers;
using SMSService.Interfaces;
using System.Net.WebSockets;
using System.Text;
using SMSService.Models;

namespace SMSService.Snippets
{
    internal class EchoSnippet
    {
        private readonly static IMessageRepository _messageRepository;
        internal static async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
           
            while (!receiveResult.CloseStatus.HasValue)
            {
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                var message = JsonConvert.DeserializeObject<Message>(receivedMessage);

                message.CreatedAt = DateTime.UtcNow;
                await _messageRepository.AddMessageAsync(message);

                string responseMessage = JsonConvert.SerializeObject(message);
                var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);

                await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
    }
}
