using CodenamesClean.Abstracts;
using System.Text;

namespace CodenamesClean.Services
{
    public class WordBank : IWordBank
    {
        private readonly IWebHostEnvironment _env;
        private readonly Dictionary<string, IReadOnlyList<string>> _cache = new(StringComparer.OrdinalIgnoreCase);

        public WordBank(IWebHostEnvironment env) => _env = env;

        public async Task<IReadOnlyList<string>> LoadAsync(string lang, CancellationToken ct = default)
        {
            if (_cache.TryGetValue(lang, out var cached)) return cached;

            var basePath = Path.Combine(_env.WebRootPath ?? "wwwroot", "lang", $"words-{lang}.txt");
            var customPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "lang", $"custom-{lang}.txt");

            var words = new List<string>();
            if (File.Exists(basePath))
                words.AddRange(await File.ReadAllLinesAsync(basePath, Encoding.UTF8, ct));

            if (File.Exists(customPath))
                words.AddRange(await File.ReadAllLinesAsync(customPath, Encoding.UTF8, ct));

            var norm = words
                .Select(w => w.Trim())
                .Where(w => w.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            _cache[lang] = norm;
            return norm;
        }
        public async Task AddWordsAsync(string lang, IEnumerable<string> words, CancellationToken ct = default)
        {
            var customPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "lang", $"custom-{lang}.txt");

            var toAdd = words
                .Select(w => w.Trim())
                .Where(w => w.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (toAdd.Count == 0) return;

            Directory.CreateDirectory(Path.GetDirectoryName(customPath)!);

            await File.AppendAllLinesAsync(customPath, toAdd, Encoding.UTF8, ct);

            // Обновляем кэш
            if (_cache.TryGetValue(lang, out var existing))
            {
                _cache[lang] = existing.Concat(toAdd)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();
            }
        }
    }
}
