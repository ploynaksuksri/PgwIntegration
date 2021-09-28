using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PgwIntegration
{
    public class PaymentBackend
    {
        private readonly PgwIntegration._2c2p.PaymentBackend _paymentBackend;

        public PaymentBackend(PgwIntegration._2c2p.PaymentBackend paymentBackend)
        {
            _paymentBackend = paymentBackend;
        }

        [FunctionName("ProcessBackend")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation($"Process backend response: {requestBody}");

            await _paymentBackend.UpdateAsync(requestBody);

            return new OkObjectResult(requestBody);
        }
    }
}