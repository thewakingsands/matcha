namespace Cafe.Matcha.Network.Universalis
{
    using Newtonsoft.Json;

    internal class UniversalisItemMateria
    {
        [JsonProperty("slotID")]
        public int SlotId { get; set; }

        [JsonProperty("materiaID")]
        public int MateriaId { get; set; }
    }
}
