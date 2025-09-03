using CodenamesClean.Abstracts;
using CodenamesClean.Enums;
using CodenamesClean.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using CodenamesClean.Services;

namespace CodenamesClean.Hubs
{
    public sealed class GameHub : Hub
    {
        private readonly IRoomStore _rooms;
        private readonly IWordBank _bank;

        public GameHub(IRoomStore rooms, IWordBank bank)
        {
            _rooms = rooms;
            _bank = bank;
        }

        public sealed record CreateArgs(GameMode Mode, string Lang);
        public sealed record JoinArgs(string RoomId, string PlayerName, Team Team, bool IsSpymaster);
        public sealed record GuessArgs(string RoomId, int CardIndex);

        public async Task<string> CreateRoom(CreateArgs args)
        {
            var roomId = RoomId.New();
            var seed = Convert.ToHexString(RandomNumberGenerator.GetBytes(8)).ToLowerInvariant();
            var words = await _bank.LoadAsync(args.Lang);

            var board = args.Mode switch
            {
                GameMode.Classic => ClassicBoardGenerator.Generate(args.Lang, seed, words, Team.Red),
                GameMode.Duet => DuetBoardGenerator.Generate(args.Lang, seed, words),
                _ => throw new ArgumentOutOfRangeException()
            };

            var room = new Room
            {
                Id = roomId,
                Mode = args.Mode,
                Board = board,
                CreatedAt = DateTime.UtcNow
            };

            await _rooms.TryAddAsync(room);
            return roomId;
        }

        public async Task Join(JoinArgs args)
        {
            var room = await _rooms.GetAsync(args.RoomId) ?? throw new HubException("Room not found");
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);

            room.Players[Context.ConnectionId] = new Player
            {
                Id = Context.ConnectionId,
                Name = args.PlayerName,
                Team = args.Team,
                IsSpymaster = args.IsSpymaster
            };

            await Clients.Caller.SendAsync("RoomState", room);
            await Clients.Group(room.Id).SendAsync("PlayersChanged", room.Players.Values);
        }

        public async Task Guess(GuessArgs args)
        {
            var room = await _rooms.GetAsync(args.RoomId) ?? throw new HubException("Room not found");

            var card = room.Board.Cards[args.CardIndex];
            if (!card.Revealed)
            {
                card.Revealed = true;
                await Clients.Group(room.Id).SendAsync("RoomState", room);
            }
        }
    }
}
