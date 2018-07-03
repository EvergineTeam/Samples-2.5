namespace Networking_P2P.Extensions
{
    public static class StringExtensions
    {
        public static string Sanitize(this string playerId)
        {
            playerId = playerId.Replace(".", string.Empty);

            return playerId;
        }
    }
}
