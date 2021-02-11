namespace Cafe.Matcha.Network.Universalis
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    internal class UniversalisItemListingsUploadRequest
    {
        [JsonProperty("worldID")]
        public int WorldId { get; set; }

        [JsonProperty("itemID")]
        public uint ItemId { get; set; }

        [JsonProperty("listings")]
        public List<UniversalisItemListingsEntry> Listings { get; set; }

        [JsonProperty("uploaderID")]
        public ulong UploaderId { get; set; }
    }
}
