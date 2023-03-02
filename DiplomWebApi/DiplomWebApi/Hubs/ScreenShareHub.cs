using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace RecordingService.Hubs
{
    public class ConnectModel
    {
        public string Caller { get; set; }
        public string RecorderId { get; set; }
    }
    public class ScreenMessage
    {
        public Guid RecorderId { get; set; }
        public string Base64 { get; set; }
    }
    [EnableCors("hubs")]
    public class ScreenShareHub: Hub
    {
        private static ConcurrentDictionary<string, ConcurrentBag<string>> concurentDictionary = new ConcurrentDictionary<string, ConcurrentBag<string>>();
        public override Task OnConnectedAsync()
        {
            var s = Context.ConnectionId;
            return base.OnConnectedAsync();
        }
        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;

            var room = concurentDictionary.FirstOrDefault(item => item.Value.Contains(connectionId));

            if (room.Key != null)
            {
                concurentDictionary[room.Key] = new ConcurrentBag<string>(room.Value.Except(new[] { connectionId }));

                await Groups.RemoveFromGroupAsync(connectionId, room.Key);

                if (concurentDictionary[room.Key].Count == 0)
                {
                    await Clients.All.SendAsync("StopSharing", room.Key);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(string recorderId)
        {
            var connectionId = Context.ConnectionId;
            await Clients.All.SendAsync("ReceiveMessage", new ConnectModel { Caller = Context.ConnectionId, RecorderId = recorderId });
            await Groups.AddToGroupAsync(connectionId, recorderId);

            if(concurentDictionary.TryGetValue(recorderId, out var connectors))
            {
                if (connectors == null)
                    connectors = new ConcurrentBag<string>();

                connectors.Add(connectionId);
            }
            else
            {
                connectors = new ConcurrentBag<string>();

                connectors.Add(connectionId);
                concurentDictionary.TryAdd(recorderId, connectors);
            }
        }

        public async Task SendMessageToCaller(string caller)
        {
            await Clients.Client(caller).SendAsync("ReceiveMessage", "Yo!");
        }

        public async Task SendScreenToCaller(ScreenMessage message)
        {
            await Clients.Group(message.RecorderId.ToString()).SendAsync("ReceiveImage", message.Base64);
        }
    }
}
