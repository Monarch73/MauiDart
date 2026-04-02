using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using DartsCounter.Models;
using DartsCounter.ViewModels;

namespace DartsCounter.Services
{
    public class DartsSyncService
    {
        private const int Port = 50001;
        private UdpClient _udpClient;
        private bool _isListening;

        public DartsSyncService()
        {
            _udpClient = new UdpClient();
        }

        // Server (Windows) sends data
        public async Task BroadcastState(object gameState)
        {
            try
            {
                var json = JsonSerializer.Serialize(gameState);
                var data = Encoding.UTF8.GetBytes(json);
                var endpoint = new IPEndPoint(IPAddress.Broadcast, Port);
                
                // Note: On some networks, broadcast might be restricted.
                // In a real app, you might want to use a specific IP.
                await _udpClient.SendAsync(data, data.Length, endpoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Broadcast error: {ex.Message}");
            }
        }

        // Client (Android) listens for data
        public void StartListening(Action<string> onDataReceived)
        {
            if (_isListening) return;
            _isListening = true;

            Task.Run(async () =>
            {
                using var listener = new UdpClient(Port);
                var groupEP = new IPEndPoint(IPAddress.Any, Port);

                while (_isListening)
                {
                    try
                    {
                        var result = await listener.ReceiveAsync();
                        var json = Encoding.UTF8.GetString(result.Buffer);
                        onDataReceived?.Invoke(json);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Listen error: {ex.Message}");
                    }
                }
            });
        }

        public void StopListening()
        {
            _isListening = false;
        }
    }
}
