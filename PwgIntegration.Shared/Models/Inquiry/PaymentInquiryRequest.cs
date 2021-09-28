namespace PwgIntegration.Shared.Models.Inquiry
{
    public class PaymentInquiryRequest : BaseRequest
    {
        public string PaymentToken { get; set; }
        public string Locale { get; set; }
    }
}