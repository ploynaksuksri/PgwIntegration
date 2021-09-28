using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PgwIntegration.Shared.Models;
using PwgIntegration.Shared;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PgwIntegration._2c2p
{
    public abstract class BasePayment<TRequest, TResponse>
    {
        protected HttpClient _httpClient;
        protected PgwOptions _options;
        protected TokenHelper _tokenHelper;
        protected ILogger _logger;
        protected PaymentRepository _paymentRepository;

        public BasePayment(
            ILogger logger,
            IHttpClientFactory httpClientFactory,
            IOptions<PgwOptions> options)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient(Constants.HttpClient2c2p);
            _options = options.Value;
            _tokenHelper = new TokenHelper(_options.SecretKey);
            _paymentRepository = new PaymentRepository(_options.Database);
        }

        protected async virtual Task<TResponse> SendRequestAsync(TRequest request, HttpMethod method, string requestUri)
        {
            var encodedPayload = CreatePayload(request, Constants.PayloadProperty);
            var requestMessage = CreateRequestMessage(method, requestUri, encodedPayload);
            var response = await _httpClient.SendAsync(requestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            // {"respCode":"9005","respDesc":"Invalid Request (9005)InvoiceNo, Description"}
            // Todo : check if respCode is "0000" before carry on

            string decodedPayload = ExtractPayloadResponse(responseContent, Constants.PayloadProperty);
            return JsonConvert.DeserializeObject<TResponse>(decodedPayload);
        }

        protected virtual string CreatePayload(TRequest request, string property)
        {
            var requestData = new Dictionary<string, string>()
            {
                { property, _tokenHelper.Encode(request) }
            };
            return JsonConvert.SerializeObject(requestData);
        }

        protected virtual HttpRequestMessage CreateRequestMessage(HttpMethod method, string requestUri, string payload)
        {
            var requestMessage = new HttpRequestMessage(method, requestUri);
            requestMessage.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            return requestMessage;
        }

        protected virtual string ExtractPayloadResponse(string response, string property)
        {
            Dictionary<string, string> responseJSON = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            string encodedPayload = responseJSON[property];
            return _tokenHelper.Decode(encodedPayload);
        }
    }
}