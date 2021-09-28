using PwgIntegration.Shared.Models.Inquiry;
using System.Threading.Tasks;

namespace PwgIntegration.Shared
{
    public interface IPaymentInquiry
    {
        Task<PaymentInquiryResponse> InquiryAsync(PaymentInquiryRequest request);
    }
}