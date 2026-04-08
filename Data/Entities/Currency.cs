using System.Text.Json.Serialization;

namespace EFCoreDeepDive.Data.Entities

{
    public class Currency
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [JsonIgnore] // Wasn't included in the tutorial but the serialzed JSON looked bad
        public virtual ICollection<BookPrice> BookPrices { get; set; }
    }
}
