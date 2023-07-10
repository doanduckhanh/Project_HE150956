namespace ProjectManagementAPI.DTO
{
    public class NewFoodDTO
    {
        public string? FoodName { get; set; }
        public decimal? FoodPrice { get; set; }
        public string? FoodImage { get; set; }
        public int? FoodStatus { get; set; }
        public int? CategoryId { get; set; }
    }
}
