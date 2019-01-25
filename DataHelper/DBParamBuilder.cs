using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace DataHelper
{
    internal class DBParamBuilder
	{
		private AssemblyProvider _assemblyProvider;

		private string _providerName;

		internal DBParamBuilder(string providerName)
		{
			_assemblyProvider = null;
			_providerName = string.Empty;
			_assemblyProvider = new AssemblyProvider(providerName);
			_providerName = providerName;
		}

		private DbParameter GetParameter()
		{
			return _assemblyProvider.Factory.CreateParameter();
		}

		internal DbParameter GetParameter(DBParameter parameter)
		{
			DbParameter parameter2 = GetParameter();
			parameter2.ParameterName = parameter.Name;
			parameter2.Value = RuntimeHelpers.GetObjectValue(parameter.Value);
			parameter2.Direction = parameter.ParamDirection;
			parameter2.DbType = parameter.Type;
			return parameter2;
		}

		internal List<DbParameter> GetParameterCollection(DBParameterCollection parameterCollection)
		{
			List<DbParameter> list = new List<DbParameter>();
			DbParameter dbParameter = null;
			foreach (DBParameter parameter in parameterCollection.Parameters)
			{
				dbParameter = GetParameter(parameter);
				list.Add(dbParameter);
			}
			return list;
		}
	}
}
