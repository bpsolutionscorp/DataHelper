using System.Configuration;
using System.Diagnostics;

namespace DataHelper
{
	internal class Configuration
	{
		private const string DEFAULT_CONNECTION_KEY = "defaultConnection";

		public static string ConnectionString => ConfigurationManager.ConnectionStrings[DefaultConnection].ConnectionString;

		public static string DefaultConnection => ConfigurationManager.AppSettings["defaultConnection"];

		public static string ProviderName => ConfigurationManager.ConnectionStrings[DefaultConnection].ProviderName;

		public static string GetConnectionString(string connectionName)
		{
			return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
		}

		public static string GetProviderName(string connectionName)
		{
			return ConfigurationManager.ConnectionStrings[connectionName].ProviderName;
		}
	}
}
