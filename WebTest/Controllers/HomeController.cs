using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PwgIntegration.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebTest.Models;

namespace WebTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient();
        }

        public IActionResult Index()
        {
            ViewData["InvoiceNo"] = $"Test{new Random().Next(9999999)}";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> TestAsync(string merchantId, decimal amount, string currencyCode, string invoiceNo)
        {
            //_client.BaseAddress = new Uri("http://localhost:7071");
            //var url = new Uri("https://bcl-payment.azurewebsites.net/api/Pay?code=3RdRa5l46jeF8/6fSlJ31Z/tNpFZp/ZYM/dS6iEwUvoe3LX5dTtmBA==");
            _client.BaseAddress = new Uri("https://bcl-payment.azurewebsites.net");
            var paymentRequest = new PaymentRequest()
            {
                MerchantId = merchantId,
                Amount = amount,
                CurrencyCode = currencyCode,
                InvoiceNo = invoiceNo
            };
            //var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/Pay");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/Pay?code=3RdRa5l46jeF8/6fSlJ31Z/tNpFZp/ZYM/dS6iEwUvoe3LX5dTtmBA==");
            requestMessage.Content = new StringContent(JObject.FromObject(paymentRequest).ToString(), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();
            JObject payloadObject = JObject.Parse(responseBody);
            return Redirect((string)payloadObject["webPaymentUrl"]);
        }

        public async Task<IActionResult> ProcessResultAsync()
        {
            string responseBody = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            string encodedPayload = responseBody.Split('=')[1];
            var data = Convert.FromBase64String(HttpUtility.UrlDecode(encodedPayload));
            string decodedPayload = Encoding.UTF8.GetString(data);
            JObject payloadObject = JObject.Parse(decodedPayload);
            ViewData["InvoiceNo"] = (string)payloadObject["invoiceNo"];
            ViewData["paymentResponse"] = decodedPayload;
            return View("Index");
        }
    }

    public class PaymentRequest
    {
        public string MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string InvoiceNo { get; set; }
    }
}