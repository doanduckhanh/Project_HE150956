using BusinessObjects.Models;

namespace ProjectManagementAPI.DTO
{
    public class CartDTO
    {
        public int RecordId { get; set; }
        public string? CartId { get; set; }
        public int? FoodId { get; set; }
        public int? Count { get; set; }
        public DateTime? DateCreated { get; set; }
        public FoodDTO? Food { get; set; }
    }
}
