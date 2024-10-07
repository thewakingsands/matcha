namespace Cafe.Matcha.Network.Universalis
{
    using Newtonsoft.Json;

    internal class UniversalisItem
    {
        [JsonProperty("pricePerUnit")]
        public int PricePerUint;

        [JsonProperty("worldName")]
        public string WorldName;

        [JsonProperty("quantity")]
        public int Quantity;

        [JsonProperty("hq")]
        public bool Hq;
    }

    internal class UniversalisQueryResponse
    {
        [JsonProperty("listings")]
        public UniversalisItem[] ListingItems;
    }
}
