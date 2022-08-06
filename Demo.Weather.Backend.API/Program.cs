using Demo.Weather.Backend.API.Database;
using Demo.Weather.Shared;

namespace Demo.Weather.Backend.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            builder.Services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddSeq(config.GetSection("Seq"));
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IDatbaseProvider, DatbaseProvider>();

            var source = "DemoWeatherBackendApi";

            /* builder.Services.AddOpenTelemetryTracing(b =>
            {
                b.AddConsoleExporter();
                b.AddSource(source);
                b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: source, serviceVersion: "1.0.0"));
                b.AddAspNetCoreInstrumentation();
                b.AddHttpClientInstrumentation();
                b.AddZipkinExporter(o => o.HttpClientFactory = () =>
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("X-MyCustomHeader", "value");
                    return client;
                });
            }); */

            OpenTelemetrySettings.Configure(builder.Services, source);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}