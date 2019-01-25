using System.Data;
using System.Runtime.CompilerServices;

namespace DataHelper
{
    public class DBParameter
	{
		private string _name;

		private ParameterDirection _paramDirection;

		private DbType _type;

		private object _value;

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public ParameterDirection ParamDirection
		{
			get
			{
				return _paramDirection;
			}
			set
			{
				_paramDirection = value;
			}
		}

		public DbType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		public object Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = RuntimeHelpers.GetObjectValue(value);
			}
		}

		public DBParameter()
		{
			_name = string.Empty;
			_value = null;
			_type = DbType.String;
			_paramDirection = ParameterDirection.Input;
		}

		public DBParameter(string name, object value)
		{
			_name = string.Empty;
			_value = null;
			_type = DbType.String;
			_paramDirection = ParameterDirection.Input;
			_name = name;
			_value = RuntimeHelpers.GetObjectValue(value);
		}

		public DBParameter(string name, object value, DbType dbType)
		{
			_name = string.Empty;
			_value = null;
			_type = DbType.String;
			_paramDirection = ParameterDirection.Input;
			_name = name;
			_value = RuntimeHelpers.GetObjectValue(value);
			_type = dbType;
		}

		public DBParameter(string name, object value, ParameterDirection paramDirection)
		{
			_name = string.Empty;
			_value = null;
			_type = DbType.String;
			_paramDirection = ParameterDirection.Input;
			_name = name;
			_value = RuntimeHelpers.GetObjectValue(value);
			_paramDirection = paramDirection;
		}

		public DBParameter(string name, object value, DbType dbType, ParameterDirection paramDirection)
		{
			_name = string.Empty;
			_value = null;
			_type = DbType.String;
			_paramDirection = ParameterDirection.Input;
			_name = name;
			_value = RuntimeHelpers.GetObjectValue(value);
			_type = dbType;
			_paramDirection = paramDirection;
		}
	}
}
