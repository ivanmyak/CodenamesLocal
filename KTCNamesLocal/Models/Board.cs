using CodenamesClean.Enums;

namespace CodenamesClean.Models
{
    public sealed class Board
    {
        public required GameMode Mode { get; init; }
        public required string Seed { get; init; }
        public required string Language { get; init; }   // "en" | "ru"
        public required IReadOnlyList<Card> Cards { get; init; }

        // Счёт
        public int RedLeft { get; set; }   // Classic
        public int BlueLeft { get; set; }  // Classic
        public int GreenLeft { get; set; } // Duet (суммарно по обеим сторонам)
    }
}
