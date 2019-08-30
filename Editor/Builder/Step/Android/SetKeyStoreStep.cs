using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Pringles
{
	public class SetKeyStoreStep : BuildStep
	{
		protected override string Tag { get { return "Set KeyStore"; } }

		//======================================
		//	変数
		//======================================
		private readonly string m_name	= null;
		private readonly string m_pass	= null;
		private readonly string m_aliasName = null;
		private readonly string m_aliasPass = null;

		//======================================
		//	関数
		//======================================

		public SetKeyStoreStep( 
			string storeName,
			string storePass,
			string aliasName,
			string aliasPass
		)
		{
			m_name		= storeName;
			m_pass      = storePass;
			m_aliasName = aliasName;
			m_aliasPass = aliasPass;
		}
		public SetKeyStoreStep(IKeyStore keyStore) : this(
			storeName: keyStore.StoreName, storePass: keyStore.StorePass,
			aliasName: keyStore.AliasName, aliasPass: keyStore.AliasPass
		)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		protected override BuildResult DoExecute()
		{
			// KeyStore設定
			SetKeyStoreInfo(m_name, m_pass, m_aliasName, m_aliasPass);

			if (string.IsNullOrEmpty(m_name))
			{
				return Warning($"StoreName is Null. Use Debug KeyStore:{Dump()}");
			}

			return Success($"{Dump()}");
		}

		/// <summary>
		/// KeyStore情報を設定します
		/// </summary>
		private void SetKeyStoreInfo(
			string keystoreName,
			string keystorePass,
			string keyaliasName,
			string keyaliasPass
		)
		{

			PlayerSettings.Android.keystoreName = ConvertKeyStorePath( keystoreName );
			PlayerSettings.Android.keystorePass = keystorePass;
			PlayerSettings.Android.keyaliasName = keyaliasName;
			PlayerSettings.Android.keyaliasPass = keyaliasPass;
		}
		/// <summary>
		/// 絶対パスに変える
		/// </summary>
		private string ConvertKeyStorePath( string keyStoreName )
		{
			if( string.IsNullOrEmpty(keyStoreName))
			{
				// 指定なしは空で返す
				return string.Empty;
			}
			var dataPath = new Uri(Environment.CurrentDirectory + "/");
			var uri = new Uri(dataPath, keyStoreName);
			if(!File.Exists(uri.LocalPath))
			{
				throw new FileNotFoundException($"{uri.LocalPath}\nPlease Relative :{dataPath.LocalPath}/ + {keyStoreName}");
			}
			return uri.LocalPath;
		}


		private string Dump()
		{
			return 
				$"{nameof(PlayerSettings.Android.keystoreName)}:{PlayerSettings.Android.keystoreName}" +
				$"{nameof(PlayerSettings.Android.keystorePass)}:{PlayerSettings.Android.keystorePass}" +
				$"{nameof(PlayerSettings.Android.keyaliasName)}:{PlayerSettings.Android.keyaliasName}" +
				$"{nameof(PlayerSettings.Android.keyaliasPass)}:{PlayerSettings.Android.keyaliasPass}"
			;
		}
	}
}