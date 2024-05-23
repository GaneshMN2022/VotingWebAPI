using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Voting.DTO;

namespace Voting.Logic.Socket {
    public class WebSocketManager {
        private readonly List<WebSocket> _sockets = new List<WebSocket>();

        public void AddSocket(WebSocket socket) {
            _sockets.Add(socket);
        }

        public void RemoveSocket(WebSocket socket) {
            _sockets.Remove(socket);
        }

        public async Task SendMessageToAllAsync(string type, object payload) {
            var message = new WebSocketMessage {
                Type = type,
                Payload = payload
            };

            var json = JsonSerializer.Serialize(message);
            var buffer = Encoding.UTF8.GetBytes(json);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var socket in _sockets) {
                if (socket.State == WebSocketState.Open) {
                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        public async Task HandleWebSocketAsync(WebSocket webSocket) {
            AddSocket(webSocket);
            var buffer = new byte[1024 * 4];

            try {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!result.CloseStatus.HasValue) {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
            } finally {
                RemoveSocket(webSocket);
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketManager", CancellationToken.None);
            }
        }
    }
}
