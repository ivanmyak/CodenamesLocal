using CodenamesClean.Models;

namespace CodenamesClean.Abstracts
{
    public interface IRoomStore
    {
        Task<Room?> GetAsync(string roomId, CancellationToken ct = default);
        Task<bool> TryAddAsync(Room room, CancellationToken ct = default);
        Task<bool> RemoveAsync(string roomId, CancellationToken ct = default);
        IAsyncEnumerable<Room> ListAsync(CancellationToken ct = default);
    }
}
