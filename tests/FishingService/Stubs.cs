// ACT-independent adapters. All parser, HTTP, memory-layout and service code is production code.
namespace Cafe.Matcha
{
    internal class Config
    {
        public static Config Instance { get; } = new Config();
        public Constant.Region Region => Constant.Region.China;
        public Models.ConfigOverlay Overlay { get; } = new Models.ConfigOverlay();
    }
    internal class Data
    {
        public static string Version = "1.2.3.4";
        public static Data Instance { get; } = new Data();
        public uint[] FishingPlaces { get; } = new uint[] { 20 };
    }
}
namespace Cafe.Matcha.Models
{
    internal class ConfigOverlay : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public string FishingFloatAuth { get; set; }
        private bool enabled;
        public bool FishingFloat { get => enabled; set { enabled = value; PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(FishingFloat))); } }
    }
}
namespace Cafe.Matcha.Network
{
    internal class State { public static State Instance { get; } = new State(); public uint FishingBait { get; set; } }
}
namespace Cafe.Matcha.Utils
{
    internal static class Helper { public static long Now => System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); }
    internal class ParsePlugin
    {
        public static ParsePlugin Instance => null;
        public System.Diagnostics.Process GetProcess() => null;
        public int?[] GetGatheringPoints() => new int?[] { null, null };
    }
}
