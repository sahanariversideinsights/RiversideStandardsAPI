using Microsoft.Extensions.Configuration;
using System.IO;

namespace StandardsApiData.Common
{
    public class AppConfiguration
    {
        private readonly string _connectionString = string.Empty;
        private readonly string _defaultProvider = string.Empty;
        public AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            _connectionString = root.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            _defaultProvider = root.GetSection("DefaultProvider").GetSection("ProviderName").Value;
        }
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }            
        }

        public string DefaultProvider
        {
            get
            {
                return _defaultProvider;
            }
        }
    }
}
