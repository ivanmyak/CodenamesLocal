namespace CodenamesClean.Abstracts
{
    public interface IWordBank
    {
        Task<IReadOnlyList<string>> LoadAsync(string lang, CancellationToken ct = default);
        Task AddWordsAsync(string lang, IEnumerable<string> words, CancellationToken ct = default);
    }
}
