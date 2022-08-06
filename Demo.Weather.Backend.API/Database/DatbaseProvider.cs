using MySqlConnector;

namespace Demo.Weather.Backend.API.Database
{
    public class DatbaseProvider : IDatbaseProvider
    {
        private readonly ILogger<DatbaseProvider> _logger;

        public DatbaseProvider(ILogger<DatbaseProvider> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation($">>> Instantiating {typeof(DatbaseProvider).FullName} class.");
        }

        public async Task<DateTime?> GetDateTime()
        {
            string connStr = GetConnectionString();
            MySqlConnection conn = new(connStr);
            // var ping = await conn.PingAsync();

            DateTime? result = null;

            try
            {
                _logger.LogInformation(">>> Attempting to connect to database");

                await conn.OpenAsync();

                _logger.LogInformation(">>> Connection to Database Successful");

                using MySqlCommand command = new("SELECT NOW();", conn);
                var nowObj = await command.ExecuteScalarAsync();

                if (nowObj != null)
                {
                    var now = Convert.ToDateTime(nowObj);
                    _logger.LogInformation($">>> Now: {now.ToString()}");
                    result = now;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
            }
            finally
            {
                _logger.LogInformation(">>> Attempting to close database connection");
                await conn.CloseAsync();
            }

            return result;
        }
        private string GetConnectionString()
        {
            return new MySqlConnectionStringBuilder
            {
                Server = "delete-me-database-1.ceiy5yfgzyfi.us-east-1.rds.amazonaws.com",
                Database = "sampledb",
                Port = 3306,
                UserID = "admin",
                Password = "qKi2zWQQi#RpYCm*Dk",
                SslMode = MySqlSslMode.Required,
                TlsVersion = "TLSv1.2"
            }.ConnectionString;
        }
    }
}
