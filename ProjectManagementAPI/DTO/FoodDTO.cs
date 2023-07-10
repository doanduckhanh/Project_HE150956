using AutoMapper;

namespace ProjectManagementAPI.DTO
{
    public class FoodDTO
    {
        public int FoodId { get; set; }
        public string? FoodName { get; set; }
        public decimal? FoodPrice { get; set; }
        public string? FoodImage { get; set; }
        public int? FoodStatus { get; set; }
        public int? CategoryId { get; set; }
        public CategoryDTO? Category { get; set; }
        
    }
}
