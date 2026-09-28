// ACT-independent host adapters; parsing, storage and export use production sources.
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
    }
}
