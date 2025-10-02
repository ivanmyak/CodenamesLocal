using CodenamesClean.Enums;

namespace CodenamesClean.Models
{
    public sealed class Card
    {
        public required string Word { get; init; }
        public bool Revealed { get; set; }

        // Classic
        public ClassicRole? ClassicRole { get; init; }

        // Duet: карта видится по-разному для сторон A/B
        public DuetMark? DuetA { get; init; }
        public DuetMark? DuetB { get; init; }
    }
}
