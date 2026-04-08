namespace EFCoreDeepDive.Data.Entities
{
    public class Language
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // This references the LanguageId column in
        // Book.cs and creates a one-to-many relationship
        // from this reference point. This is not necessary
        // but provides two way navigation.
        // This will blow if if JSONSERIALIZED.
        public ICollection<Book> Books { get; set; }

 
    }
}
