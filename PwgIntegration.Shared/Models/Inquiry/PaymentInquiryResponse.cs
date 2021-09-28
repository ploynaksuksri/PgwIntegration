namespace PwgIntegration.Shared.Models.Inquiry
{
    public class PaymentInquiryResponse : BaseResponse
    {
        public string MerchantId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}