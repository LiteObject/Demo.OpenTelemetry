namespace Demo.Weather.Shared
{
    using Microsoft.Extensions.DependencyInjection;
    using OpenTelemetry.Exporter;
    using OpenTelemetry.Metrics;
    using OpenTelemetry.Resources;
    using OpenTelemetry.Trace;

    public static class OpenTelemetrySettings
    {
        public static void Configure(IServiceCollection services, string source)
        {
            // shared Resource to use for both OpenTelemetry metrics AND tracing
            var resource = ResourceBuilder.CreateDefault().AddService("Demo-AspNet-With-OpenTelemetry", "Demo");

            services.AddOpenTelemetryTracing(b =>
            {
                // receive traces from our own custom sources
                b.AddSource(source);
                b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: source, serviceVersion: "1.0.0"));

                b.AddConsoleExporter(options => options.Targets = ConsoleExporterOutputTargets.Debug);

                // uses the default Jaeger settings... For more:
                // https://www.jaegertracing.io/docs/1.21/opentelemetry/
                // https://opentelemetry.io/docs/instrumentation/net/exporters/
                // https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry.Exporter.Jaeger
                /*b.AddJaegerExporter(options =>
                {
                    // The Jaeger Collector HTTP endpoint (default http://localhost:14268). Used for HttpBinaryThrift protocol.
                    // options.Endpoint = new Uri("http://localhost:14268");

                    // The Jaeger Agent host (default localhost). Used for UdpCompactThrift protocol
                    options.AgentHost = "localhost";

                    // The Jaeger Agent port (default 6831). Used for UdpCompactThrift protocol.
                    options.AgentPort = 6831;
                }
                ); */

                b.AddJaegerExporter();

                /*b.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri("http://localhost:55680");
                    options.Protocol = OtlpExportProtocol.HttpProtobuf;
                }
                );*/

                //b.AddZipkinExporter(o => o.HttpClientFactory = () =>
                //{
                //    HttpClient client = new HttpClient();
                //    client.DefaultRequestHeaders.Add("X-MyCustomHeader", "value");
                //    return client;
                //});

                // decorate our service name so we can find it when we look inside Jaeger
                b.SetResourceBuilder(resource);

                // receive traces from built-in sources
                b.AddAspNetCoreInstrumentation();
                b.AddHttpClientInstrumentation();
                b.AddSqlClientInstrumentation();
            });

            services.AddOpenTelemetryMetrics(b =>
            {
                // add prometheus exporter
                b.AddPrometheusExporter();

                // receive metrics from our own custom sources
                b.AddMeter(TelemetryConstants.MyAppSource);

                // decorate our service name so we can find it when we look inside Prometheus
                b.SetResourceBuilder(resource);

                // receive metrics from built-in sources
                b.AddHttpClientInstrumentation();
                b.AddAspNetCoreInstrumentation();
            });
        }
    }
}
