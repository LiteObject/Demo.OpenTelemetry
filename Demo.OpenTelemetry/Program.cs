using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Demo.OpenTelemetry
{
    class Program
    {
        private static readonly ActivitySource MyActivitySource = new ActivitySource("MyCompany.MyProduct.MyLibrary");
        static void Main(string[] args)
        {
            /*using var openTelemetry = Sdk.CreateTracerProviderBuilder()
                        .AddSource("MyCompany.MyProduct.MyLibrary")
                        .SetResource(Resources.CreateServiceResource(Environment.GetEnvironmentVariable("LS_SERVICE_NAME"),
                                serviceVersion: Environment.GetEnvironmentVariable("LS_SERVICE_VERSION")))
                        .AddOtlpExporter(opt =>
                        {
                            opt.Endpoint = "ingest.lightstep.com:443";
                            opt.Headers = new Metadata
                            {
                            { "KEiKzwFMELaaS35Hiu0S+1vZzv+5Y3Rn1ahU3lVQ26OQohmVBLPH/nEV93JptyjwiiE2LhiQy0YnPj+fDyqhv73wsgj0vUj2uTNtZv5+",
                                    Environment.GetEnvironmentVariable("LS_ACCESS_TOKEN")}
                            };
                            opt.Credentials = new SslCredentials();
                        })
                        .Build();*/

            using (var activity = MyActivitySource.StartActivity("SayHello"))
            {
                activity?.SetTag("foo", 1);
                activity?.SetTag("bar", "Hello, World!");
                activity?.SetTag("baz", new int[] { 1, 2, 3 });
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // add to your existing service collection
            /*services.AddOpenTelemetryTracing((builder) => builder
              .AddAspNetCoreInstrumentation()
              .AddHttpClientInstrumentation()
              .AddOtlpExporter(otlpOptions =>
              {
                  otlpOptions.ServiceName = Environment.GetEnvironmentVariable("LS_SERVICE_NAME");
                  otlpOptions.Endpoint = "ingest.lightstep.com:443";
                  otlpOptions.Headers = new Metadata
                    {
            { "lightstep-access-token", Environment.GetEnvironmentVariable("LS_ACCESS_TOKEN")}
                    };
                  otlpOptions.Credentials = new SslCredentials();
              }));*/
        }
    }
}