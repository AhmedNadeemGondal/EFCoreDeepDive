using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreDeepDive.Data

{
    public class BookPrice
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        // See comments in Book.cs
        // One-to-many with Book entity
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        // Many-to-one with Currency entity
        public int CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }
    }
}