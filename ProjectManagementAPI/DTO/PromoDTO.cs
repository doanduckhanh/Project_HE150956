namespace ProjectManagementAPI.DTO
{
    public class PromoDTO
    {
        public string PromoCode { get; set; } = null!;
        public string? PromoValue { get; set; }
        public string? PromoDescribe { get; set; }
        public bool? PromoStatus { get; set; }
    }
}
