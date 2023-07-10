namespace ProjectManagementAPI.DTO
{
    public class NewOrderDTO
    {
        public DateTime? OrderDate { get; set; }
        public string? PromoCode { get; set; }
        public string? UserName { get; set; }
        public decimal? Total { get; set; }
        public int? UserId { get; set; }
    }
}
