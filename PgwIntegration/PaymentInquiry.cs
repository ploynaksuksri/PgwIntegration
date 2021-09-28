using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PwgIntegration.Shared;
using PwgIntegration.Shared.Models.Inquiry;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace PgwIntegration
{
    public class PaymentInquiry
    {
        private readonly IPaymentInquiry _paymentInquiry;

        public PaymentInquiry(IPaymentInquiry paymentInquiry)
        {
            _paymentInquiry = paymentInquiry;
        }

        [FunctionName("Inquiry")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            PaymentInquiryRequest request = JsonConvert.DeserializeObject<PaymentInquiryRequest>(requestBody);
            log.LogInformation($"Payment inquiry request: {requestBody}");

            var result = await _paymentInquiry.InquiryAsync(request);

            return new OkObjectResult(result);
        }
    }
}