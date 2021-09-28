using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PgwIntegration._2c2p;
using PgwIntegration.Shared.Models;
using PwgIntegration.Shared;
using System;

[assembly: FunctionsStartup(typeof(PgwIntegration.Startup))]

namespace PgwIntegration
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<PgwOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(Constants.PgwSection).Bind(settings);
                });

            builder.Services.AddHttpClient(Constants.HttpClient2c2p, c =>
            {
                c.BaseAddress = new Uri(builder.GetContext().Configuration.GetSection($"{Constants.PgwSection}:baseUrl").Value);
                c.DefaultRequestHeaders.Add("Accept", "text/plain");
            });

            builder.Services.AddTransient<IPayment, RedirectPayment>();
            builder.Services.AddTransient<IPaymentInquiry, PgwIntegration._2c2p.PaymentInquiry>();
            builder.Services.AddTransient<PgwIntegration._2c2p.PaymentBackend>();
        }
    }
}