namespace nabegemu.Hubs;
public class LobbyHub : Hub
{
    public async Task AddNewPlayer(string playerName, string gameCode)
    {
        await Clients.All.SendAsync("NewPlayerAdded", playerName);
    }
}
