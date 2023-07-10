namespace ProjectManagementAPI.DTO
{
    public class NewOrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? FoodId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
