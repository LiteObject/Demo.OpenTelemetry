namespace Demo.Weather.Backend.API.Database
{
    public interface IDatbaseProvider
    {
        public Task<DateTime?> GetDateTime();
    }
}
