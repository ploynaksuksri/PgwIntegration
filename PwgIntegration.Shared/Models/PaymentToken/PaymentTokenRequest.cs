namespace PwgIntegration.Shared.Models.PaymentToken
{
    public class PaymentTokenRequest : BaseRequest
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string FrontendReturnUrl { get; set; } = "http://localhost:56494/Home/ProcessResult";
        public string BackendReturnUrl { get; set; }
    }
}