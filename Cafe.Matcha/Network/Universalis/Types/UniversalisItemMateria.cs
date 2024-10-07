namespace Cafe.Matcha.Network.Universalis
{
    using Newtonsoft.Json;

    /// <summary>
    /// A Universalis API structure.
    /// </summary>
    internal class UniversalisItemMateria
    {
        /// <summary>
        /// Gets or sets the item slot ID.
        /// </summary>
        [JsonProperty("slotID")]
        public int SlotId { get; set; }

        /// <summary>
        /// Gets or sets the materia ID.
        /// </summary>
        [JsonProperty("materiaID")]
        public int MateriaId { get; set; }
    }
}
