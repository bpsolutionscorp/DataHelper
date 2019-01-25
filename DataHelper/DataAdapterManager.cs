using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace DataHelper
{
    internal class DataAdapterManager
	{
		private AssemblyProvider _assemblyProvider;

		private CommandBuilder _commandBuilder;

		private string _providerName;

		public DataAdapterManager()
		{
			_commandBuilder = null;
			_assemblyProvider = null;
			_providerName = string.Empty;
			_providerName = Configuration.GetProviderName(Configuration.DefaultConnection);
			_commandBuilder = new CommandBuilder(_providerName);
			_assemblyProvider = new AssemblyProvider(_providerName);
		}

		public DataAdapterManager(string providerName)
		{
			_commandBuilder = null;
			_assemblyProvider = null;
			_providerName = string.Empty;
			_providerName = providerName;
			_commandBuilder = new CommandBuilder(providerName);
			_assemblyProvider = new AssemblyProvider(_providerName);
		}

		private DbDataAdapter GetDataAdapter(IDbCommand command)
		{
			DbDataAdapter dbDataAdapter = _assemblyProvider.Factory.CreateDataAdapter();
			dbDataAdapter.SelectCommand = (DbCommand)command;
			return dbDataAdapter;
		}

		internal DbDataAdapter GetDataAdapter(string sqlCommand, IDbConnection connection)
		{
			return GetDataAdapter(sqlCommand, connection, CommandType.Text);
		}

		internal DbDataAdapter GetDataAdapter(string sqlCommand, IDbConnection connection, CommandType commandType)
		{
			IDbCommand command = _commandBuilder.GetCommand(sqlCommand, connection, commandType);
			return GetDataAdapter(command);
		}

		internal DbDataAdapter GetDataAdapter(string sqlCommand, IDbConnection connection, DBParameter param, CommandType commandType)
		{
			IDbCommand command = _commandBuilder.GetCommand(sqlCommand, connection, param, commandType);
			return GetDataAdapter(command);
		}

		internal DbDataAdapter GetDataAdapter(string sqlCommand, IDbConnection connection, DBParameterCollection paramCollection, CommandType commandType)
		{
			IDbCommand command = _commandBuilder.GetCommand(sqlCommand, connection, paramCollection, commandType);
			return GetDataAdapter(command);
		}

		internal DataTable GetDataTable(string sqlCommand, IDbConnection connection)
		{
			return GetDataTable(sqlCommand, new DBParameterCollection(), connection, string.Empty, CommandType.Text);
		}

		internal DataTable GetDataTable(string sqlCommand, IDbConnection connection, CommandType commandType)
		{
			return GetDataTable(sqlCommand, new DBParameterCollection(), connection, string.Empty, commandType);
		}

		internal DataTable GetDataTable(string sqlCommand, IDbConnection connection, string tableName, CommandType commandType)
		{
			return GetDataTable(sqlCommand, new DBParameterCollection(), connection, tableName, commandType);
		}

		internal DataTable GetDataTable(string sqlCommand, DBParameter param, IDbConnection connection, string tableName, CommandType commandType)
		{
			DBParameterCollection dBParameterCollection = new DBParameterCollection();
			dBParameterCollection.Add(param);
			return GetDataTable(sqlCommand, dBParameterCollection, connection, tableName, commandType);
		}

		internal DataTable GetDataTable(string sqlCommand, DBParameterCollection paramCollection, IDbConnection connection, string tableName, CommandType commandType)
		{
			DataTable dataTable = null;
			dataTable = ((Operators.CompareString(tableName, string.Empty, TextCompare: false) == 0) ? new DataTable() : new DataTable(tableName));
			IDbCommand dbCommand = null;
			dbCommand = ((paramCollection == null) ? _commandBuilder.GetCommand(sqlCommand, connection, commandType) : ((paramCollection.Parameters.Count <= 0) ? _commandBuilder.GetCommand(sqlCommand, connection, commandType) : _commandBuilder.GetCommand(sqlCommand, connection, paramCollection, commandType)));
			DbDataAdapter dbDataAdapter = (DbDataAdapter)GetDataAdapter(dbCommand).GetType().GetConstructor(new Type[1]
			{
				dbCommand.GetType()
			}).Invoke(new object[1]
			{
				dbCommand
			});
			MethodInfo method = dbDataAdapter.GetType().GetMethod("Fill", new Type[1]
			{
				typeof(DataTable)
			});
			try
			{
				method.Invoke(dbDataAdapter, new object[1]
				{
					dataTable
				});
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return dataTable;
		}
	}
}
