// ACT-independent adapters; the packet parser, handler and DTOs are production code.
namespace Cafe.Matcha
{
    internal class Config
    {
        public static Config Instance { get; } = new Config();
        public Constant.Region Region => Constant.Region.China;
    }
}
namespace Cafe.Matcha.Utils
{
    internal static class Helper
    {
        public static long Now => System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public static string ReadString(byte[] data, int offset, int length)
        {
            var end = System.Array.IndexOf(data, (byte)0, offset, length);
            return System.Text.Encoding.UTF8.GetString(data, offset, end < 0 ? length : end - offset);
        }
    }
}
