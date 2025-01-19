using Moq;
using nabegemu.Database.Interfaces;
using nabegemu.Database.Models;
using nabegemu.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;

namespace nabegemu.Tests.Hubs;

public class LobbyHubTests
{
    private readonly Mock<IGameRepository> _mockGameRepository;
    private readonly Mock<IHubContext<LobbyHub>> _mockHubContext;
    private readonly Mock<ISingleClientProxy> _mockSingleClientProxy;
    private readonly Mock<IHubClients> _mockHubClients;
    private readonly LobbyHub _lobbyHub;

    public LobbyHubTests()
    {
        _mockGameRepository = new Mock<IGameRepository>();
        _mockHubContext = new Mock<IHubContext<LobbyHub>>();
        _mockSingleClientProxy = new Mock<ISingleClientProxy>();
        _mockHubClients = new Mock<IHubClients>();

        var mockClients = new Mock<IHubCallerClients>();
        mockClients.Setup(clients => clients.Caller).Returns(_mockSingleClientProxy.Object);
        mockClients.Setup(clients => clients.Group(It.IsAny<string>())).Returns(_mockSingleClientProxy.Object);

        var mockGroups = new Mock<IGroupManager>();

        _mockHubContext.Setup(context => context.Clients).Returns(_mockHubClients.Object);
        _mockHubContext.Setup(context => context.Groups).Returns(mockGroups.Object);

        _lobbyHub = new LobbyHub(_mockGameRepository.Object)
        {
            Clients = mockClients.Object,
            Groups = mockGroups.Object,
            Context = new HubCallerContextMock()
        };
    }

    [Fact]
    public async Task AddNewPlayer_ShouldAddPlayerToGame()
    {
        // Arrange
        var gameId = 1;
        var playerName = "TestPlayer";
        var gameCode = "1234";
        var player = new Player { Id = Guid.NewGuid(), Name = playerName };
        var game = new Game { GameId = gameId, Players = new List<Player> { player } };

        _mockGameRepository.Setup(repo => repo.AddPlayerToGame(It.IsAny<int>(), It.IsAny<string>())).Returns(player);
        _mockGameRepository.Setup(repo => repo.GetGame(It.IsAny<int>())).Returns(game);

        // Act
        await _lobbyHub.AddNewPlayer(playerName, gameCode);

        // Assert
        Assert.Multiple(() =>
        {
            _mockSingleClientProxy.Verify(client => client.SendCoreAsync("SetPlayerDataInSession", It.Is<object[]>(o => o[0] == player), default), Times.Once);
            _mockSingleClientProxy.Verify(client => client.SendCoreAsync("NewPlayerAdded", It.Is<object[]>(o => o[0] == game), default), Times.Once);
        });
    }

    [Fact]
    public async Task CreateNewGame_ShouldCreateGameAndAddPlayer()
    {
        // Arrange
        var playerName = "TestPlayer";
        var player = new Player { Id = Guid.NewGuid(), Name = playerName };
        var game = new Game { GameId = 1, Players = new List<Player> { player } };

        _mockGameRepository.Setup(repo => repo.CreateGame(It.IsAny<Player>())).Returns(game);

        // Act
        await _lobbyHub.CreateNewGame(playerName);

        // Assert
        Assert.Multiple(() =>
        {
            _mockSingleClientProxy.Verify(client => client.SendCoreAsync("SetPlayerDataInSession", It.Is<object[]>(o => o[0] == player), default), Times.Once);
            _mockSingleClientProxy.Verify(client => client.SendCoreAsync("NewGameCreated", It.Is<object[]>(o => o[0] == game), default), Times.Once);
        });
    }

    [Fact]
    public async Task PrepKitchen_ShouldSendPrepKitchenComplete()
    {
        // Arrange
        var gameCode = 1234;
        var playerId = Guid.NewGuid();
        var player = new Player { Id = playerId };

        _mockGameRepository.Setup(repo => repo.GetPlayer(It.IsAny<int>(), It.IsAny<Guid>())).Returns(player);

        // Act
        await _lobbyHub.PrepKitchen(gameCode, playerId);

        // Assert
        _mockSingleClientProxy.Verify(client => client.SendCoreAsync("PrepKitchenComplete", It.Is<object[]>(o => o[0] == player), default), Times.Once);
    }

    [Fact]
    public async Task SetActivePlayerInSession_ShouldSetActiveAndInactivePlayers()
    {
        // Arrange
        var gameCode = 1234;
        var activePlayer = new Player { Id = Guid.NewGuid(), IsActivePlayer = true };
        var inactivePlayer = new Player { Id = Guid.NewGuid(), IsActivePlayer = false };
        var players = new List<Player> { activePlayer, inactivePlayer };

        _mockGameRepository.Setup(repo => repo.GetActivePlayer(It.IsAny<int>())).Returns(activePlayer);
        _mockGameRepository.Setup(repo => repo.GetPlayers(It.IsAny<int>())).Returns(players);

        // Act
        await _lobbyHub.SetActivePlayerInSession(gameCode);

        // Assert
        Assert.Multiple(() =>
        {
            _mockSingleClientProxy.Verify(client => client.SendCoreAsync("SetActivePlayerInSession", It.Is<object[]>(o => (bool)o[0] == true), default), Times.Once);
            _mockSingleClientProxy.Verify(client => client.SendCoreAsync("SetInactivePlayerInSession", It.Is<object[]>(o => (bool)o[0] == false), default), Times.Once);
        });
    }
}

public class HubCallerContextMock : HubCallerContext
{
    public override string ConnectionId => "TestConnectionId";

    public override string UserIdentifier => "TestUserIdentifier";

    public override ClaimsPrincipal User => new ClaimsPrincipal();

    public override IFeatureCollection Features => new FeatureCollection();

    public override CancellationToken ConnectionAborted => CancellationToken.None;

    public override IDictionary<object, object?> Items => new Dictionary<object, object?>();

    public override void Abort() { }
}
