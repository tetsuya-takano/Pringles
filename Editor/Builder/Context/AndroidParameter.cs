using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
	public interface IKeyStore : IBuildParameter
	{
		string StoreName { get; }
		string StorePass { get; }
		string AliasName { get; }
		string AliasPass { get; }

	}
	public sealed class KeyStoreParameter : IKeyStore
	{
		public string StoreName { get; }

		public string StorePass { get; }

		public string AliasName { get; }

		public string AliasPass { get; }


		public KeyStoreParameter( 
			string store_name, string store_pass,
			string alias_name, string alias_pass
		)
		{
			StoreName = store_name;
			StorePass = store_pass;
			AliasName = alias_name;
			AliasPass = alias_pass;
		}
	}

	public interface ISplitBinary : IBuildParameter
	{
		bool IsUseObb { get; }
	}

	public sealed class SplitBinaryParameter : ISplitBinary
	{
		public bool IsUseObb { get; }

		public SplitBinaryParameter( bool isUse)
		{
			IsUseObb = isUse;
		}
	}
}