using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

public class WebSocketServerConnectionManager
{
    private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly int _notificationInterval = 5000; 
    public ConcurrentDictionary<string, WebSocket> GetSockets()
    {
        return _sockets;
    }

    public string AddSocket(WebSocket socket)
    {
        string connectionId = Guid.NewGuid().ToString();
        _sockets.TryAdd(connectionId, socket);
        Console.WriteLine($"Connection added: {connectionId}");
        return connectionId;
    }

    public bool RemoveSocket(string connectionId)
    {
        return _sockets.TryRemove(connectionId, out var socket);
    }

  
    public void StartPeriodicNotifications()
    {
        var thread = new Thread(() =>
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
       
                NotifyAllClients("Notification");
                Thread.Sleep(_notificationInterval);  
            }
        });
        thread.IsBackground = true; 
        thread.Start();
    }

   
    private void NotifyAllClients(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        foreach (var socket in _sockets.Values)
        {
            if (socket.State == WebSocketState.Open)
            {
                socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            }
        }
    }

 
    public void StopPeriodicNotifications()
    {
        _cancellationTokenSource.Cancel();
    }
}
