using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DataHelper
{
    internal class CommandBuilder
	{
		private AssemblyProvider _assemblyProvider;

		private DBParamBuilder _paramBuilder;

		private string _providerName;

		public CommandBuilder()
		{
			_providerName = string.Empty;
			_paramBuilder = null;
			_assemblyProvider = null;
			_providerName = Configuration.GetProviderName(Configuration.DefaultConnection);
			_paramBuilder = new DBParamBuilder(_providerName);
			_assemblyProvider = new AssemblyProvider(_providerName);
		}

		public CommandBuilder(string providerName)
		{
			_providerName = string.Empty;
			_paramBuilder = null;
			_assemblyProvider = null;
			_providerName = providerName;
			_paramBuilder = new DBParamBuilder(providerName);
			_assemblyProvider = new AssemblyProvider(_providerName);
		}

		private IDbCommand GetCommand()
		{
			return _assemblyProvider.Factory.CreateCommand();
		}

		internal IDbCommand GetCommand(string commandText, IDbConnection connection)
		{
			return GetCommand(commandText, connection, CommandType.Text);
		}

		internal IDbCommand GetCommand(string commandText, IDbConnection connection, DBParameter parameter)
		{
			return GetCommand(commandText, connection, parameter, CommandType.Text);
		}

		internal IDbCommand GetCommand(string commandText, IDbConnection connection, DBParameterCollection parameterCollection)
		{
			return GetCommand(commandText, connection, parameterCollection, CommandType.Text);
		}

		internal IDbCommand GetCommand(string commandText, IDbConnection connection, CommandType commandType)
		{
			IDbCommand command = GetCommand();
			command.CommandText = commandText;
			command.Connection = connection;
			command.CommandType = commandType;
			return command;
		}

		internal IDbCommand GetCommand(string commandText, IDbConnection connection, DBParameter parameter, CommandType commandType)
		{
			IDataParameter parameter2 = _paramBuilder.GetParameter(parameter);
			IDbCommand command = GetCommand(commandText, connection, commandType);
			command.Parameters.Add(parameter2);
			return command;
		}

		internal IDbCommand GetCommand(string commandText, IDbConnection connection, DBParameterCollection parameterCollection, CommandType commandType)
		{
			List<DbParameter> parameterCollection2 = _paramBuilder.GetParameterCollection(parameterCollection);
			IDbCommand command = GetCommand(commandText, connection, commandType);
			foreach (DbParameter item in parameterCollection2)
			{
				command.Parameters.Add(item);
			}
			return command;
		}
	}
}
