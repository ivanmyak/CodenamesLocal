using CodenamesClean.Enums;
using CodenamesClean.Models;
using System.Security.Cryptography;

namespace CodenamesClean.Services
{
    public static class DuetBoardGenerator
    {
        // Приближённый ключ Duet:
        // - Каждая сторона видит 9 "Green", 3 "Assassin", остальное "Neutral".
        // - 1 убийца общий, по одному убийце у каждой стороны (и он попадает в "Green" у другой).
        // - Пересечение "Green" ~3 клетки — итого уникальных зелёных ~15.
        public static Board Generate(string lang, string seed, IReadOnlyList<string> words)
        {
            var rng = new Random(HashSeed(seed));
            var chosen = words.OrderBy(_ => rng.Next()).Take(25).ToArray();

            var idx = Enumerable.Range(0, 25).OrderBy(_ => rng.Next()).ToList();

            var greensA = idx.Take(9).ToHashSet();
            var greensB = idx.Skip(6).Take(9).ToHashSet();       // пересечение ~3
            var commonAssassin = idx[24];

            // Убийца A — на зелёной клетке у B, и наоборот
            var assassinA = greensB.Except(new[] { commonAssassin }).OrderBy(_ => rng.Next()).First();
            var assassinB = greensA.Except(new[] { commonAssassin }).OrderBy(_ => rng.Next()).First();

            var cards = new List<Card>(25);
            for (int i = 0; i < 25; i++)
            {
                var a = DuetMark.Neutral;
                var b = DuetMark.Neutral;

                if (i == commonAssassin) { a = DuetMark.Assassin; b = DuetMark.Assassin; }
                else
                {
                    if (i == assassinA) a = DuetMark.Assassin;
                    else if (greensA.Contains(i)) a = DuetMark.Green;

                    if (i == assassinB) b = DuetMark.Assassin;
                    else if (greensB.Contains(i)) b = DuetMark.Green;
                }

                cards.Add(new Card { Word = chosen[i], DuetA = a, DuetB = b, Revealed = false });
            }

            var totalGreens = cards.Count(c => c.DuetA == DuetMark.Green || c.DuetB == DuetMark.Green);

            return new Board
            {
                Mode = GameMode.Duet,
                Seed = seed,
                Language = lang,
                Cards = cards,
                RedLeft = 0,
                BlueLeft = 0,
                GreenLeft = totalGreens
            };
        }

        static int HashSeed(string seed)
        {
            var h = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(seed));
            return BitConverter.ToInt32(h, 0);
        }
    }
}
