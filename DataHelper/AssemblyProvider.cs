using System.Data.Common;

namespace DataHelper
{
	internal class AssemblyProvider
	{
		private string _providerName;

		internal DbProviderFactory Factory => DbProviderFactories.GetFactory(_providerName);

		public AssemblyProvider()
		{
			_providerName = string.Empty;
			_providerName = Configuration.GetProviderName(Configuration.DefaultConnection);
		}

		public AssemblyProvider(string providerName)
		{
			_providerName = string.Empty;
			_providerName = providerName;
		}
	}
}
