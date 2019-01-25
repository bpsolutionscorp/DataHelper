using System.Collections.Generic;

namespace DataHelper
{
	public class DBParameterCollection
	{
		private List<DBParameter> _parameterCollection;

		internal List<DBParameter> Parameters => _parameterCollection;

		public DBParameterCollection()
		{
			_parameterCollection = new List<DBParameter>();
		}

		public void Add(DBParameter parameter)
		{
			_parameterCollection.Add(parameter);
		}

		public void Remove(DBParameter parameter)
		{
			_parameterCollection.Remove(parameter);
		}

		public void RemoveAll()
		{
			_parameterCollection.RemoveRange(0, checked(_parameterCollection.Count - 1));
		}

		public void RemoveAt(int index)
		{
			_parameterCollection.RemoveAt(index);
		}
	}
}
