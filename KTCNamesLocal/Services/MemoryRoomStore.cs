using CodenamesClean.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;
using CodenamesClean.Abstracts;

namespace CodenamesClean.Services
{
    public sealed class MemoryRoomStore : IRoomStore
    {
        private readonly ConcurrentDictionary<string, Room> _rooms = new();

        public Task<Room?> GetAsync(string roomId, CancellationToken ct = default)
            => Task.FromResult(_rooms.TryGetValue(roomId, out var r) ? r : null);

        public Task<bool> TryAddAsync(Room room, CancellationToken ct = default)
            => Task.FromResult(_rooms.TryAdd(room.Id, room));

        public Task<bool> RemoveAsync(string roomId, CancellationToken ct = default)
            => Task.FromResult(_rooms.TryRemove(roomId, out _));

        public async IAsyncEnumerable<Room> ListAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
        {
            foreach (var r in _rooms.Values)
            {
                if (ct.IsCancellationRequested) yield break;
                yield return r;
                await Task.Yield();
            }
        }
    }
}
