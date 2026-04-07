namespace EFCoreDeepDive.Data
{
    public class BookDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int NoOfPages { get; set; }
        public bool IsActive { get; set; }
        public int LanguageId { get; set; }
    }
}
