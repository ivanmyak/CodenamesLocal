using CodenamesClean.Enums;
using CodenamesClean.Models;
using System.Security.Cryptography;

namespace CodenamesClean.Services
{
    public static class ClassicBoardGenerator
    {
        // 25 карт: 9 у стартующей команды, 8 у другой, 7 нейтральных, 1 убийца.
        public static Board Generate(string lang, string seed, IReadOnlyList<string> words, Team starting)
        {
            var rng = new Random(HashSeed(seed));
            var chosen = words.OrderBy(_ => rng.Next()).Take(25).ToArray();

            var red = starting == Team.Red ? 9 : 8;
            var blue = starting == Team.Blue ? 9 : 8;
            var neutral = 25 - red - blue - 1; // 7

            var roles = Enumerable.Repeat(ClassicRole.RedAgent, red)
                .Concat(Enumerable.Repeat(ClassicRole.BlueAgent, blue))
                .Concat(Enumerable.Repeat(ClassicRole.Neutral, neutral))
                .Concat(new[] { ClassicRole.Assassin })
                .OrderBy(_ => rng.Next())
                .ToArray();

            var cards = new List<Card>(25);
            foreach (var (w, role) in chosen.Zip(roles))
            {
                cards.Add(new Card { Word = w, ClassicRole = role, Revealed = false });
            }

            return new Board
            {
                Mode = GameMode.Classic,
                Seed = seed,
                Language = lang,
                Cards = cards,
                RedLeft = roles.Count(r => r == ClassicRole.RedAgent),
                BlueLeft = roles.Count(r => r == ClassicRole.BlueAgent),
                GreenLeft = 0
            };
        }

        static int HashSeed(string seed)
        {
            var h = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(seed));
            return BitConverter.ToInt32(h, 0);
        }
    }
}
