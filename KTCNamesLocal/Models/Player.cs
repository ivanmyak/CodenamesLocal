using CodenamesClean.Enums;

namespace CodenamesClean.Models
{
    public sealed class Player
    {
        public required string Id { get; init; }       // ConnectionId
        public required string Name { get; init; }
        public Team Team { get; set; }                 // Classic: Red/Blue; Duet: None
        public bool IsSpymaster { get; set; }
    }
}
