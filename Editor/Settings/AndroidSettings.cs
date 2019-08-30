using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// Android用のビルド設定
	/// </summary>
	public sealed class AndroidSettings : BuildSettings, IKeyStore, ISplitBinary
	{
		//============================
		//	class
		//============================
		public class KeySotre
		{
			public string Name { get; private set; }
			public string Pass { get; private set; }
			public KeySotre( string name, string pass )
			{
				Name = name;
				Pass = pass;
			}
		}
		public class KeyAlias
		{
			public string Name { get; private set; }
			public string Pass { get; private set; }
			public KeyAlias( string name, string pass )
			{
				Name = name;
				Pass = pass;
			}
		}

		//実際に使用するキーストア
		public string StoreName { get; } = "";
		public string StorePass { get; } = "";
		public string AliasName { get; } = "";
		public string AliasPass { get; } = "";
		public bool IsUseObb { get; } = false;

		//============================
		//	プロパティ
		//============================
		public override BuildTarget Target { get { return BuildTarget.Android; } }
		public override BuildTargetGroup Group { get { return BuildTargetGroup.Android; } }


		//============================
		//	関数
		//============================


		public AndroidSettings(
			string productName,
			string bundleId,
			string locationPath,
			string[] includeScenes,
			BuildOptions buildOptions,

			KeySotre keyStore,
			KeyAlias keyAlias,
			bool isObbBuild
			)
			: base( productName, bundleId, locationPath, includeScenes, buildOptions )
		{
			StoreName = keyStore.Name;
			StorePass = keyStore.Pass;
			AliasName = keyAlias.Name;
			AliasPass = keyAlias.Pass;
			IsUseObb = isObbBuild;
		}
	}
}