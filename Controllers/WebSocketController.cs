using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;


namespace WebSocketApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private readonly WebSocketServerConnectionManager _connectionManager;

        public WebSocketController(WebSocketServerConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        [HttpPost("send/{connectionId}")]
        public async Task<IActionResult> SendMessageToClient(string connectionId, [FromBody] string message)
        {
            if (_connectionManager.GetSockets().TryGetValue(connectionId, out var socket))
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                return Ok($"Message sent to client {connectionId}");
            }
            return NotFound($"Connection {connectionId} not found");
        }
    }
}