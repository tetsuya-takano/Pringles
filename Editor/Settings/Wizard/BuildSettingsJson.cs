using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pringles
{ 

	/// <summary>
	/// ビルド設定ファイル
	/// 共通部分だけ
	/// </summary>
	[Serializable]
	public sealed class BuildSettingsJson : IBuildSettings, ISerializationCallbackReceiver
	{
		[SerializeField] private BuildTarget m_target = 0;
		[SerializeField] private BuildTargetGroup m_group = 0;
		[SerializeField] private string m_productName = string.Empty;
		[SerializeField] private string m_bundleId = string.Empty;
		[SerializeField] private BuildOptions m_options = BuildOptions.None;
		[SerializeField] private string[] m_levels = new string[] { };
		[SerializeField] private string m_output = string.Empty;

		public BuildOptions Option => m_options;
		public string ProductName => m_productName;
		public string BundleId => m_bundleId;
		public string LocationPath => m_output;
		public string[] Levels => m_levels;

		public BuildTargetGroup Group => m_group;
		public BuildTarget Target => m_target;

		public static BuildSettingsJson Create( 
			BuildTarget target,
			BuildTargetGroup group,
			string bundleId, 
			string productName, 
			string output, 
			BuildOptions option, 
			string[] levels)
		{
			var json = new BuildSettingsJson();
			json.m_target = target;
			json.m_group = group;
			json.m_productName = productName;
			json.m_bundleId = bundleId;
			json.m_options = option;
			json.m_levels = levels;
			json.m_output = output;

			return json;
		}

		public static BuildSettingsJson Read( string path )
		{
			var contents = File.ReadAllText( path );
			return JsonUtility.FromJson<BuildSettingsJson>(contents);
		}

		public void OnAfterDeserialize()
		{
			// 読み込んだときにパスに変換
			m_levels = m_levels.Select( c => AssetDatabase.GUIDToAssetPath( c )).ToArray();
		}

		public void OnBeforeSerialize() { }
	}

	/// <summary>
	/// プロダクト名とBundleId
	/// </summary>
	[Serializable]
	public sealed class RomIdentifierJson : IBundleId, IProductName
	{
		//================================
		// SerializeField
		//================================
		[SerializeField] private string m_productName = string.Empty;
		[SerializeField] private string m_bundleId = string.Empty;

		//================================
		// プロパティ
		//================================
		public string ProductName => m_productName;

		public string BundleId => m_bundleId;


		public static RomIdentifierJson Create( string appName, string bundleId )
		{
			var obj = new RomIdentifierJson();
			obj.m_productName = appName;
			obj.m_bundleId = bundleId;

			return obj;
		}
		public static RomIdentifierJson Read(string path )
		{
			var content = File.ReadAllText(path);
			return JsonUtility.FromJson<RomIdentifierJson>(content);
		}
	}

	/// <summary>
	/// ビルドシーン
	/// </summary>
	[Serializable]
	public sealed class BuildSceneJson : IBuildScenes
	{
		//================================
		// SerializeField
		//================================
		[SerializeField] private string[] m_levels = new string[0];

		//================================
		// プロパティ
		//================================
		public string[] Levels => m_levels;

		public static BuildSceneJson Create( string[] scenes)
		{
			var obj = new BuildSceneJson();
			obj.m_levels = scenes;
			return obj;
		}

		public static BuildSceneJson Read(string path)
		{
			var content = File.ReadAllText(path);
			return JsonUtility.FromJson<BuildSceneJson>(content);
		}
	}

	/// <summary>
	/// ビルド設定
	/// </summary>
	[Serializable]
	public sealed class AppBuildSettingsJson : IBuildOption,ISerializationCallbackReceiver
	{
		[SerializeField] private bool m_developmentBuild = true;
		[SerializeField] private bool m_allowDebugging = true;
		[SerializeField] private bool m_connectProfiler = true;

		public BuildOptions Option { get; private set; }

		public void OnAfterDeserialize()
		{
			if( m_developmentBuild)
			{
				Option |= BuildOptions.Development;
			}
			if ( m_allowDebugging)
			{
				Option |= BuildOptions.AllowDebugging;
			}
			if (m_connectProfiler)
			{
				Option |= BuildOptions.ConnectWithProfiler;
			}
		}

		public void OnBeforeSerialize() { }

		public static AppBuildSettingsJson Create( bool isDevelopment, bool allowDebugging, bool connectProfiler )
		{
			var obj = new AppBuildSettingsJson();
			obj.m_developmentBuild = isDevelopment;
			obj.m_connectProfiler = connectProfiler;
			obj.m_allowDebugging = allowDebugging;
			return obj;
		}


		public static AppBuildSettingsJson Read(string path)
		{
			var content = File.ReadAllText(path);
			return JsonUtility.FromJson<AppBuildSettingsJson>(content);
		}

	}

	[Serializable]
	public sealed class AndroidKeyStoreJson : IKeyStore
	{

		[SerializeField] private string m_storeName = string.Empty;
		[SerializeField] private string m_storePass = string.Empty;
		[SerializeField] private string m_aliasName = string.Empty;
		[SerializeField] private string m_aliasPass = string.Empty;

		public string StoreName => m_storeName;

		public string StorePass => m_storePass;

		public string AliasName => m_aliasName;

		public string AliasPass => m_aliasPass;
	}

	[Serializable]
	public sealed class AndroidBuildSettingsJson : ISplitBinary
	{
		[SerializeField] private bool m_isSplit = false;
		public bool IsUseObb => m_isSplit;
	}
}