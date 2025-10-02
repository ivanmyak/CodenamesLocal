namespace CodenamesClean.Abstracts
{
    public static class RoomId
    {
        public static string New() => Convert.ToHexString(Guid.NewGuid().ToByteArray()).ToLowerInvariant()[..8];
    }
}
