using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EFCoreDeepDive.Data.Entities
{
    public class Book
    {

        public int Id { get; set; } // DB Column
        public string Title { get; set; } // DB Column
        public string Description { get; set; } // DB Column
        public int NoOfPages { get; set; } // DB Column
        public bool IsActive { get; set; } // DB Column
        public DateTime CreatedOn { get; set; } // DB Column

        // EF Core uses Convention. It sees LanguageId and Language and
        // automatically assumes they are paired. No attributes needed
        // but recommended (by me) as shown below.
        //The following line actually creates the FK column for LanguageId

        public int LanguageId { get; set; } // DB Column

        [ForeignKey("LanguageId")] // This is explicit relationship assignment
        // This suggests taht one book can only have one language in this schema
        // This is a reference navigation property as it has a complex type
        [JsonIgnore]
        public Language? Language { get; set; }

        public int? AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        [JsonIgnore]
        public Author Author { get; set; }
    }
}
