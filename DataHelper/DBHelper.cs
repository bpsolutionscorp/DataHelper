using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace DataHelper
{
    public class DBHelper
	{
		private AssemblyProvider _assemblyProvider;

		private CommandBuilder _commandBuilder;

		private IDbConnection _connection;

		private ConnectionManager _connectionManager;

		private DataAdapterManager _dbAdapterManager;

		private string _providerName;

		public string ConnectionString => _connectionManager.ConnectionString;

		public string Database
		{
			get
			{
				IDbConnection dbConnection = _assemblyProvider.Factory.CreateConnection();
				dbConnection.ConnectionString = ConnectionString;
				return dbConnection.Database;
			}
		}

		public string Provider => _providerName;

		public DBHelper(string connectionString, string providerName)
		{
			_assemblyProvider = null;
			_commandBuilder = null;
			_connection = null;
			_connectionManager = null;
			_dbAdapterManager = null;
			_providerName = string.Empty;
			_connectionManager = new ConnectionManager(connectionString, providerName);
			_commandBuilder = new CommandBuilder(providerName);
			_dbAdapterManager = new DataAdapterManager(providerName);
			_connection = _connectionManager.GetConnection();
			_providerName = _connectionManager.ProviderName;
			_assemblyProvider = new AssemblyProvider(_providerName);
		}

		public IDbTransaction BeginTransaction()
		{
			return GetConnObject().BeginTransaction();
		}

		public void CommitTransaction(IDbTransaction transaction)
		{
			try
			{
				transaction.Commit();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DisposeCommand(IDbCommand command)
		{
			if (command != null)
			{
				if (command.Connection != null)
				{
					command.Connection.Close();
					command.Connection.Dispose();
				}
				command.Dispose();
			}
		}

		public IDataReader ExecuteDataReader(string commandText)
		{
			return ExecuteDataReader(commandText, GetConnObject(), CommandType.Text);
		}

		public IDataReader ExecuteDataReader(string commandText, IDbConnection connection)
		{
			return ExecuteDataReader(commandText, connection, CommandType.Text);
		}

		public IDataReader ExecuteDataReader(string commandText, DBParameter param, IDbTransaction transaction)
		{
			return ExecuteDataReader(commandText, param, transaction, CommandType.Text);
		}

		public IDataReader ExecuteDataReader(string commandText, DBParameterCollection paramCollection, IDbTransaction transaction)
		{
			return ExecuteDataReader(commandText, paramCollection, transaction, CommandType.Text);
		}

		public IDataReader ExecuteDataReader(string commandText, IDbConnection connection, DBParameter param)
		{
			return ExecuteDataReader(commandText, connection, param, CommandType.Text);
		}

		public IDataReader ExecuteDataReader(string commandText, IDbConnection connection, DBParameterCollection paramCollection)
		{
			return ExecuteDataReader(commandText, connection, paramCollection, CommandType.Text);
		}

		public IDataReader ExecuteDataReader(string commandText, IDbConnection connection, CommandType commandType)
		{
			return ExecuteDataReader(commandText, connection, new DBParameterCollection(), commandType);
		}

		public IDataReader ExecuteDataReader(string commandText, DBParameter param, IDbTransaction transaction, CommandType commandType)
		{
			DBParameterCollection dBParameterCollection = new DBParameterCollection();
			dBParameterCollection.Add(param);
			return ExecuteDataReader(commandText, dBParameterCollection, transaction, commandType);
		}

		public IDataReader ExecuteDataReader(string commandText, DBParameterCollection paramCollection, IDbTransaction transaction, CommandType commandType)
		{
			IDataReader dataReader = null;
			IDbConnection connection = transaction.Connection;
			IDbCommand command = _commandBuilder.GetCommand(commandText, connection, paramCollection, commandType);
			command.Transaction = transaction;
			dataReader = command.ExecuteReader();
			command.Dispose();
			return dataReader;
		}

		public IDataReader ExecuteDataReader(string commandText, IDbConnection connection, DBParameter param, CommandType commandType)
		{
			DBParameterCollection dBParameterCollection = new DBParameterCollection();
			dBParameterCollection.Add(param);
			return ExecuteDataReader(commandText, connection, dBParameterCollection, commandType);
		}

		public IDataReader ExecuteDataReader(string commandText, IDbConnection connection, DBParameterCollection paramCollection, CommandType commandType)
		{
			IDataReader dataReader = null;
			IDbCommand command = _commandBuilder.GetCommand(commandText, connection, paramCollection, commandType);
			dataReader = command.ExecuteReader();
			command.Dispose();
			return dataReader;
		}

		public DataSet ExecuteDataSet(string commandText)
		{
			return ExecuteDataSet(commandText, new DBParameterCollection(), CommandType.Text);
		}

		public DataSet ExecuteDataSet(string commandText, DBParameter param)
		{
			DBParameterCollection dBParameterCollection = new DBParameterCollection();
			dBParameterCollection.Add(param);
			return ExecuteDataSet(commandText, dBParameterCollection);
		}

		public DataSet ExecuteDataSet(string commandText, DBParameterCollection paramCollection)
		{
			return ExecuteDataSet(commandText, paramCollection, CommandType.Text);
		}

		public DataSet ExecuteDataSet(string commandText, CommandType commandType)
		{
			return ExecuteDataSet(commandText, new DBParameterCollection(), commandType);
		}

		public DataSet ExecuteDataSet(string commandText, DBParameter param, CommandType commandType)
		{
			DBParameterCollection dBParameterCollection = new DBParameterCollection();
			dBParameterCollection.Add(param);
			return ExecuteDataSet(commandText, dBParameterCollection, commandType);
		}

		public DataSet ExecuteDataSet(string commandText, DBParameterCollection paramCollection, CommandType commandType)
		{
			DataSet dataSet = new DataSet();
			IDbConnection connection = _connectionManager.GetConnection();
			IDataAdapter dataAdapter = _dbAdapterManager.GetDataAdapter(commandText, connection, paramCollection, commandType);
			try
			{
				dataAdapter.Fill(dataSet);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
					connection.Dispose();
				}
			}
			return dataSet;
		}

		public DataTable ExecuteDataTable(string commandText)
		{
			return ExecuteDataTable(commandText, string.Empty, CommandType.Text);
		}

		public DataTable ExecuteDataTable(string commandText, DBParameter param)
		{
			return ExecuteDataTable(commandText, string.Empty, param, CommandType.Text);
		}

		public DataTable ExecuteDataTable(string commandText, DBParameterCollection paramCollection)
		{
			return ExecuteDataTable(commandText, string.Empty, paramCollection, CommandType.Text);
		}

		public DataTable ExecuteDataTable(string commandText, CommandType commandType)
		{
			return ExecuteDataTable(commandText, string.Empty, commandType);
		}

		public DataTable ExecuteDataTable(string commandText, string tableName)
		{
			return ExecuteDataTable(commandText, tableName, CommandType.Text);
		}

		public DataTable ExecuteDataTable(string commandText, DBParameter param, CommandType commandType)
		{
			return ExecuteDataTable(commandText, string.Empty, param, commandType);
		}

		public DataTable ExecuteDataTable(string commandText, DBParameterCollection paramCollection, CommandType commandType)
		{
			return ExecuteDataTable(commandText, string.Empty, paramCollection, commandType);
		}

		public DataTable ExecuteDataTable(string commandText, string tableName, DBParameter param)
		{
			return ExecuteDataTable(commandText, tableName, param, CommandType.Text);
		}

		public DataTable ExecuteDataTable(string commandText, string tableName, DBParameterCollection paramCollection)
		{
			return ExecuteDataTable(commandText, tableName, paramCollection, CommandType.Text);
		}

		public DataTable ExecuteDataTable(string commandText, string tableName, CommandType commandType)
		{
			return ExecuteDataTable(commandText, tableName, new DBParameterCollection(), commandType);
		}

		public DataTable ExecuteDataTable(string commandText, string tableName, DBParameter param, CommandType commandType)
		{
			DBParameterCollection dBParameterCollection = new DBParameterCollection();
			dBParameterCollection.Add(param);
			return ExecuteDataTable(commandText, tableName, dBParameterCollection, commandType);
		}

		public DataTable ExecuteDataTable(string commandText, string tableName, DBParameterCollection paramCollection, CommandType commandType)
		{
			DataTable result = new DataTable();
			IDbConnection dbConnection = null;
			try
			{
				dbConnection = _connectionManager.GetConnection();
				result = _dbAdapterManager.GetDataTable(commandText, paramCollection, dbConnection, tableName, commandType);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (dbConnection != null)
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}
			}
			return result;
		}

		public int ExecuteNonQuery(string commandText)
		{
			return ExecuteNonQuery(commandText, (IDbTransaction)null);
		}

		public int ExecuteNonQuery(string commandText, DBParameter param)
		{
			return ExecuteNonQuery(commandText, param, null);
		}

		public int ExecuteNonQuery(string commandText, DBParameterCollection paramCollection)
		{
			return ExecuteNonQuery(commandText, paramCollection, null);
		}

		public int ExecuteNonQuery(string commandText, CommandType commandType)
		{
			return ExecuteNonQuery(commandText, (IDbTransaction)null, commandType);
		}

		public int ExecuteNonQuery(string commandText, IDbTransaction transaction)
		{
			return ExecuteNonQuery(commandText, transaction, CommandType.Text);
		}

		public int ExecuteNonQuery(string commandText, DBParameter param, CommandType commandType)
		{
			return ExecuteNonQuery(commandText, param, null, commandType);
		}

		public int ExecuteNonQuery(string commandText, DBParameter param, IDbTransaction transaction)
		{
			return ExecuteNonQuery(commandText, param, transaction, CommandType.Text);
		}

		public int ExecuteNonQuery(string commandText, DBParameterCollection paramCollection, CommandType commandType)
		{
			return ExecuteNonQuery(commandText, paramCollection, null, commandType);
		}

		public int ExecuteNonQuery(string commandText, DBParameterCollection paramCollection, IDbTransaction transaction)
		{
			return ExecuteNonQuery(commandText, paramCollection, transaction, CommandType.Text);
		}

		public int ExecuteNonQuery(string commandText, IDbTransaction transaction, CommandType commandType)
		{
			return ExecuteNonQuery(commandText, new DBParameterCollection(), transaction, commandType);
		}

		public int ExecuteNonQuery(string commandText, DBParameter param, IDbTransaction transaction, CommandType commandType)
		{
			DBParameterCollection dBParameterCollection = new DBParameterCollection();
			dBParameterCollection.Add(param);
			return ExecuteNonQuery(commandText, dBParameterCollection, transaction, commandType);
		}

		public int ExecuteNonQuery(string commandText, DBParameterCollection paramCollection, IDbTransaction transaction, CommandType commandType)
		{
			int result = 0;
			IDbConnection dbConnection = (transaction != null) ? transaction.Connection : _connectionManager.GetConnection();
			IDbCommand command = _commandBuilder.GetCommand(commandText, dbConnection, paramCollection, commandType);
			command.Transaction = transaction;
			try
			{
				result = command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if ((transaction == null && dbConnection != null) ? true : false)
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}
				command?.Dispose();
			}
			return result;
		}

		public object ExecuteScalar(string commandText)
		{
			return ExecuteScalar(commandText, (IDbTransaction)null);
		}

		public object ExecuteScalar(string commandText, DBParameter param)
		{
			return ExecuteScalar(commandText, param, null);
		}

		public object ExecuteScalar(string commandText, DBParameterCollection paramCollection)
		{
			return ExecuteScalar(commandText, paramCollection, null);
		}

		public object ExecuteScalar(string commandText, CommandType commandType)
		{
			return ExecuteScalar(commandText, (IDbTransaction)null, commandType);
		}

		public object ExecuteScalar(string commandText, IDbTransaction transaction)
		{
			return ExecuteScalar(commandText, transaction, CommandType.Text);
		}

		public object ExecuteScalar(string commandText, DBParameter param, CommandType commandType)
		{
			return ExecuteScalar(commandText, param, null, commandType);
		}

		public object ExecuteScalar(string commandText, DBParameter param, IDbTransaction transaction)
		{
			return ExecuteScalar(commandText, param, transaction, CommandType.Text);
		}

		public object ExecuteScalar(string commandText, DBParameterCollection paramCollection, CommandType commandType)
		{
			return ExecuteScalar(commandText, paramCollection, null, commandType);
		}

		public object ExecuteScalar(string commandText, DBParameterCollection paramCollection, IDbTransaction transaction)
		{
			return ExecuteScalar(commandText, paramCollection, transaction, CommandType.Text);
		}

		public object ExecuteScalar(string commandText, IDbTransaction transaction, CommandType commandType)
		{
			return ExecuteScalar(commandText, new DBParameterCollection(), transaction, commandType);
		}

		public object ExecuteScalar(string commandText, DBParameter param, IDbTransaction transaction, CommandType commandType)
		{
			DBParameterCollection dBParameterCollection = new DBParameterCollection();
			dBParameterCollection.Add(param);
			return ExecuteScalar(commandText, dBParameterCollection, transaction, commandType);
		}

		public object ExecuteScalar(string commandText, DBParameterCollection paramCollection, IDbTransaction transaction, CommandType commandType)
		{
			object result = null;
			IDbConnection dbConnection = (transaction != null) ? transaction.Connection : _connectionManager.GetConnection();
			IDbCommand command = _commandBuilder.GetCommand(commandText, dbConnection, paramCollection, commandType);
			command.Transaction = transaction;
			try
			{
				result = RuntimeHelpers.GetObjectValue(command.ExecuteScalar());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if ((transaction == null && dbConnection != null) ? true : false)
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}
				command?.Dispose();
			}
			return result;
		}

		public IDbCommand GetCommand(string commandText)
		{
			return GetCommand(commandText, CommandType.Text);
		}

		public IDbCommand GetCommand(string commandText, DBParameter parameter)
		{
			return GetCommand(commandText, parameter, CommandType.Text);
		}

		public IDbCommand GetCommand(string commandText, DBParameterCollection parameterCollection)
		{
			return GetCommand(commandText, parameterCollection, CommandType.Text);
		}

		public IDbCommand GetCommand(string commandText, CommandType commandType)
		{
			IDbConnection connection = _connectionManager.GetConnection();
			return _commandBuilder.GetCommand(commandText, connection, commandType);
		}

		public IDbCommand GetCommand(string commandText, IDbTransaction transaction)
		{
			return GetCommand(commandText, transaction, CommandType.Text);
		}

		public IDbCommand GetCommand(string commandText, DBParameter parameter, CommandType commandType)
		{
			IDbConnection connection = _connectionManager.GetConnection();
			return _commandBuilder.GetCommand(commandText, connection, parameter, commandType);
		}

		public IDbCommand GetCommand(string commandText, DBParameter parameter, IDbTransaction transaction)
		{
			DBParameterCollection dBParameterCollection = new DBParameterCollection();
			dBParameterCollection.Add(parameter);
			return GetCommand(commandText, dBParameterCollection, transaction, CommandType.Text);
		}

		public IDbCommand GetCommand(string commandText, DBParameterCollection parameterCollection, CommandType commandType)
		{
			IDbConnection connection = _connectionManager.GetConnection();
			return _commandBuilder.GetCommand(commandText, connection, parameterCollection, commandType);
		}

		public IDbCommand GetCommand(string commandText, DBParameterCollection parameterCollection, IDbTransaction transaction)
		{
			return GetCommand(commandText, parameterCollection, transaction, CommandType.Text);
		}

		public IDbCommand GetCommand(string commandText, IDbTransaction transaction, CommandType commandType)
		{
			IDbConnection connection = (transaction != null) ? transaction.Connection : _connectionManager.GetConnection();
			return _commandBuilder.GetCommand(commandText, connection, commandType);
		}

		public IDbCommand GetCommand(string commandText, DBParameterCollection parameterCollection, IDbTransaction transaction, CommandType commandType)
		{
			IDbConnection connection = (transaction != null) ? transaction.Connection : _connectionManager.GetConnection();
			return _commandBuilder.GetCommand(commandText, connection, parameterCollection, commandType);
		}

		public IDbConnection GetConnObject()
		{
			return _connectionManager.GetConnection();
		}

		public object GetParameterValue(int index, IDbCommand command)
		{
			IDataParameter dataParameter = (IDataParameter)command.Parameters[index];
			return dataParameter.Value;
		}

		public object GetParameterValue(string parameterName, IDbCommand command)
		{
			IDataParameter dataParameter = (IDataParameter)command.Parameters[parameterName];
			return dataParameter.Value;
		}

		public void RollbackTransaction(IDbTransaction transaction)
		{
			try
			{
				transaction.Rollback();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
