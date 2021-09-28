namespace PgwIntegration.Shared.Models
{
    public class PgwOptions
    {
        public string BaseUrl { get; set; }
        public string PaymentTokenUrl { get; set; }
        public string PaymentInquiryUrl { get; set; }
        public string MerchantId { get; set; }
        public string SecretKey { get; set; }
        public string Database { get; set; }
    }
}