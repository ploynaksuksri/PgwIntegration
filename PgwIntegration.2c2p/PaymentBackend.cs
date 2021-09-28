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
    public class PaymentBackend : BasePayment<dynamic, dynamic>
    {
        public PaymentBackend(ILogger logger, IHttpClientFactory httpClientFactory, IOptions<PgwOptions> options)
            : base(logger, httpClientFactory, options) { }

        public async Task UpdateAsync(string jsonPayload)
        {
            var decodedPayload = ExtractPayloadResponse(jsonPayload, Constants.PayloadProperty);
            var response = JsonConvert.DeserializeObject<PaymentInquiryResponse>(decodedPayload);
            await _paymentRepository.UpdateResponse(response.InvoiceNo, decodedPayload);
        }
    }
}