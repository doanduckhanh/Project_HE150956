namespace ProjectManagementAPI.DTO
{
    public class NewCartDTO
    {
        public string? CartId { get; set; }
        public int? FoodId { get; set; }
        public int? Count { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
