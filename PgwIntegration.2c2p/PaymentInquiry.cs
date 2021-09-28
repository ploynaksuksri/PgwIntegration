using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PgwIntegration.Shared.Models;
using PwgIntegration.Shared;
using PwgIntegration.Shared.Models.Inquiry;
using System.Net.Http;
using System.Threading.Tasks;

namespace PgwIntegration._2c2p
{
    public class PaymentInquiry : BasePayment<PaymentInquiryRequest, PaymentInquiryResponse>, IPaymentInquiry
    {
        public PaymentInquiry(ILogger logger, IHttpClientFactory httpClientFactory, IOptions<PgwOptions> options)
            : base(logger, httpClientFactory, options) { }

        public async Task<PaymentInquiryResponse> InquiryAsync(PaymentInquiryRequest request)
        {
            request.MerchantId = _options.MerchantId;
            request.PaymentToken = await _paymentRepository.GetPaymentToken(request.InvoiceNo);

            return await SendRequestAsync(request, HttpMethod.Post, _options.PaymentInquiryUrl);
        }
    }

    
}