using Microsoft.AspNetCore.SignalR;

namespace FrontApiRealTime.Hubs.Common;

public class GeneralHub : Hub {
    public async Task<string> Ping(string user, string message) {
        await Clients.Caller.SendAsync("done", user, message);
        return "Pong";
    }
}