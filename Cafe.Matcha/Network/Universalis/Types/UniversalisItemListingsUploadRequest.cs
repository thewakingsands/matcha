namespace Cafe.Matcha.Network.Universalis
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// A Universalis API structure.
    /// </summary>
    internal class UniversalisItemListingsUploadRequest
    {
        /// <summary>
        /// Gets or sets the world ID.
        /// </summary>
        [JsonProperty("worldID")]
        public uint WorldId { get; set; }

        /// <summary>
        /// Gets or sets the item ID.
        /// </summary>
        [JsonProperty("itemID")]
        public uint ItemId { get; set; }

        /// <summary>
        /// Gets or sets the list of available items.
        /// </summary>
        [JsonProperty("listings")]
        public List<UniversalisItemListingsEntry> Listings { get; set; }

        /// <summary>
        /// Gets or sets the uploader ID.
        /// </summary>
        [JsonProperty("uploaderID")]
        public string UploaderId { get; set; }
    }
}
