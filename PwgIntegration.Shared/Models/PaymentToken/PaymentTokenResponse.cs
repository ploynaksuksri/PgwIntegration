namespace PwgIntegration.Shared.Models.PaymentToken
{
    public class PaymentTokenResponse : BaseResponse
    {
        public string WebPaymentUrl { get; set; }
        public string PaymentToken { get; set; }
    }
}