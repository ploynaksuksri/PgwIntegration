using PwgIntegration.Shared.Models.PaymentToken;
using System.Threading.Tasks;

namespace PwgIntegration.Shared
{
    public interface IPayment
    {
        Task<PaymentTokenResponse> GetPaymentToken(PaymentTokenRequest request);
    }
}