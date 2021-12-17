using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebApplication3.Models
{
    public class Database
    {
        SqlConnection con;

        public Database()
        {
            var configuation = GetConfiguration();
            con = new SqlConnection(configuation.GetSection("Data").GetSection("ConnectionString").Value);
        }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public SqlConnection database()
        {
            var configuation = GetConfiguration();
            con = new SqlConnection(configuation.GetSection("Data").GetSection("ConnectionString").Value);
            return con;
        }
    }
}
