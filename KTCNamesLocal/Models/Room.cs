using CodenamesClean.Enums;
using System.Collections.Concurrent;

namespace CodenamesClean.Models
{
    public sealed class Room
    {
        public required string Id { get; init; }
        public required GameMode Mode { get; init; }
        public required Board Board { get; set; }
        public required DateTime CreatedAt { get; init; }
        public string CreatorConnectionId { get; set; } = ""; // Ключ Owner-а
        public bool Finished { get; set; }

        // Classic: чей ход (Red/Blue)
        public Team Turn { get; set; } = Team.Red;

        // Подключённые игроки
        public ConcurrentDictionary<string, Player> Players { get; } = new();
    }
}
