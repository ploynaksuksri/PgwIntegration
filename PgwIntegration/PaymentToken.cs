using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PwgIntegration.Shared;
using PwgIntegration.Shared.Models.PaymentToken;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PgwIntegration
{
    public class PaymentToken
    {
        private readonly IPayment _payment;

        public PaymentToken(IPayment payment)
        {
            _payment = payment;
        }

        [FunctionName("Pay")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            PaymentTokenRequest request = JsonConvert.DeserializeObject<PaymentTokenRequest>(requestBody);
            log.LogInformation($"Payment token request: {requestBody}");

            var result = await _payment.GetPaymentToken(request);

            // To do - save payment token along with invoice no

            return new OkObjectResult(result);
        }
    }
}