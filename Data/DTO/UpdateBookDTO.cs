namespace EFCoreDeepDive.Data.DTO
{
    public class UpdateBookDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? NoOfPages { get; set; }
        public bool? IsActive { get; set; }
        public int? LanguageId { get; set; }
    }
}
