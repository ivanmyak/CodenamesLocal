using CodenamesClean.Enums;
using CodenamesClean.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;


namespace CodenamesClean.Services
{
    public class GameService : IAsyncDisposable
    {
        private readonly NavigationManager _nav;
        private HubConnection? _hub;

        // События для подписки в компонентах
        public event Action<Room>? RoomStateChanged;
        public event Action<IEnumerable<Player>>? PlayersChanged;

        public bool IsConnected => _hub?.State == HubConnectionState.Connected;

        public GameService(NavigationManager nav)
        {
            _nav = nav;
        }

        public async Task ConnectAsync()
        {
            if (_hub is not null && IsConnected) return;

            _hub = new HubConnectionBuilder()
                .WithUrl(_nav.ToAbsoluteUri("/hub/game"))
                .WithAutomaticReconnect()
                .Build();

            // Подписки на события от сервера
            _hub.On<Room>("RoomState", room => RoomStateChanged?.Invoke(room));
            _hub.On<IEnumerable<Player>>("PlayersChanged", players => PlayersChanged?.Invoke(players));

            await _hub.StartAsync();
        }

        public async Task<string> CreateRoomAsync(GameMode mode, string lang)
        {
            await ConnectAsync();
            return await _hub!.InvokeAsync<string>("CreateRoom", new { Mode = mode, Lang = lang });
        }

        public async Task JoinRoomAsync(string roomId, string playerName, Team team, bool isSpymaster)
        {
            await ConnectAsync();
            await _hub!.InvokeAsync("Join", new { RoomId = roomId, PlayerName = playerName, Team = team, IsSpymaster = isSpymaster });
        }

        public async Task GuessAsync(string roomId, int cardIndex)
        {
            if (!IsConnected) return;
            await _hub!.InvokeAsync("Guess", new { RoomId = roomId, CardIndex = cardIndex });
        }

        public async ValueTask DisposeAsync()
        {
            if (_hub is not null)
            {
                await _hub.DisposeAsync();
            }
        }
    }
}
