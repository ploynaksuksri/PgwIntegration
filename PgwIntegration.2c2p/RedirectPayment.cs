using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PgwIntegration.Shared.Models;
using PwgIntegration.Shared;
using PwgIntegration.Shared.Models.PaymentToken;
using System.Net.Http;
using System.Threading.Tasks;

namespace PgwIntegration._2c2p
{
    public class RedirectPayment : BasePayment<PaymentTokenRequest, PaymentTokenResponse>, IPayment
    {
        public RedirectPayment(ILogger logger, IHttpClientFactory httpClientFactory, IOptions<PgwOptions> options)
            : base(logger, httpClientFactory, options)
        {
        }

        public async Task<PaymentTokenResponse> GetPaymentToken(PaymentTokenRequest request)
        {
            // Modify request
            request.MerchantId = _options.MerchantId;
            request.Description = "Description";

            var response = await SendRequestAsync(request, HttpMethod.Post, _options.PaymentTokenUrl);
            await _paymentRepository.AddPayemntTokenResponse(request.InvoiceNo, response);

            // Todo - Check if status is 0000 before decode the token
            // Status code 0000 means the transaction is completed, but doesn't mean successful
            return response;
        }
    }
}