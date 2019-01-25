using System;
using System.Data;

namespace DataHelper
{
    internal class ConnectionManager
	{
		private AssemblyProvider _assemblyProvider;

		private string _connectionName;

		private string _connectionString;

		private string _providerName;

		internal string ConnectionString => _connectionString;

		internal string ProviderName => _providerName;

		internal ConnectionManager()
		{
			_connectionName = string.Empty;
			_connectionString = string.Empty;
			_providerName = string.Empty;
			_assemblyProvider = null;
			_connectionName = Configuration.DefaultConnection;
			_connectionString = Configuration.ConnectionString;
			_providerName = Configuration.ProviderName;
			_assemblyProvider = new AssemblyProvider(_providerName);
		}

		internal ConnectionManager(string connectionName)
		{
			_connectionName = string.Empty;
			_connectionString = string.Empty;
			_providerName = string.Empty;
			_assemblyProvider = null;
			_connectionName = connectionName;
			_connectionString = Configuration.GetConnectionString(connectionName);
			_providerName = Configuration.GetProviderName(connectionName);
			_assemblyProvider = new AssemblyProvider(_providerName);
		}

		internal ConnectionManager(string connectionString, string providerName)
		{
			_connectionName = string.Empty;
			_connectionString = string.Empty;
			_providerName = string.Empty;
			_assemblyProvider = null;
			_connectionString = connectionString;
			_providerName = providerName;
			_connectionName = string.Empty;
			_assemblyProvider = new AssemblyProvider(_providerName);
		}

		internal IDbConnection GetConnection()
		{
			IDbConnection dbConnection = _assemblyProvider.Factory.CreateConnection();
			dbConnection.ConnectionString = _connectionString;
			try
			{
				dbConnection.Open();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dbConnection;
		}
	}
}
