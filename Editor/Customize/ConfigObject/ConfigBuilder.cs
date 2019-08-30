using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pringles
{
	public abstract class ConfigBuilder<TFormat, TReplaceFormat, TObject> : ConfigBuilder
		where TFormat : new()
		where TObject : ConfigObject<TFormat>
		where TReplaceFormat : ReplaceFormat
	{
		protected override string BuildEmptyFormat()
		{
			return EditorJsonUtility.ToJson(new TFormat(), true);
		}
		protected override ConfigObject CreateInstance()
		{
			return ConfigObject.CreateInstance<TObject>();
		}
		protected override object GetDeserializeObject()
		{
			return Config.Deserialize<TFormat>();
		}

		public void SetValue( Action<TReplaceFormat> getRule ) => getRule?.Invoke( Format as TReplaceFormat );
	}
	/// <summary>
	/// Configファイルを作成する方
	/// </summary>
	public abstract class ConfigBuilder : ScriptableObject
	{
		//===================================
		// SerializeField
		//===================================
		[SerializeField] DefaultAsset m_configDirectory = default;
		[SerializeField] private TextAsset m_template = default;
		[SerializeField] private ReplaceFormat m_rule = default;
		[SerializeField] private ConfigObject m_targetObject = default;

		//===================================
		// プロパティ
		//===================================
		protected ConfigObject Config => m_targetObject;
		protected ReplaceFormat Format => m_rule;

		//===================================
		// 関数
		//===================================

		public void Build()
		{
			// テンプレート変換
			var contents = m_rule.Replace( m_template.text );
			Debug.Log(contents);
			// 暗号化とかとか
			m_targetObject.Serialize( contents );

			// 変換戻しテスト
			var _ = GetDeserializeObject();

			EditorUtility.SetDirty( m_targetObject );
			// 再インポート
			var p = AssetDatabase.GetAssetPath( m_targetObject );
			AssetDatabase.ImportAsset( p );
			AssetDatabase.SaveAssets();
		}

		public static T Builder<T>() where T : ConfigBuilder
		{
			var path = AssetDatabase
				.FindAssets($"t:{typeof(T).Name}")
				.Select(guid => AssetDatabase.GUIDToAssetPath(guid))
				.FirstOrDefault();

			return AssetDatabase.LoadAssetAtPath<T>( path );
		}


		#region Custom Editor
		
		[CustomEditor(typeof(ConfigBuilder), true)]
		private sealed class Inspector : Editor
		{
			/// <summary>
			/// OnGUI
			/// </summary>
			public override void OnInspectorGUI()
			{
				DrawDefaultInspector();
				DrawBuildButton();
			}

			private void DrawBuildButton()
			{
				if (GUILayout.Button("Create New Object"))
				{
					(target as ConfigBuilder)?.CreateConfigObject();
				}
				if (GUILayout.Button("Create Template"))
				{
					(target as ConfigBuilder)?.CreateTemplate();
				}
				if (GUILayout.Button("Build"))
				{
					(target as ConfigBuilder)?.Build();
				}
				if (GUILayout.Button("Dump"))
				{
					(target as ConfigBuilder)?.Dump();
				}
			}
		}

		//=========================================================
		// 
		//=========================================================
		protected void CreateTemplate()
		{
			// テンプレート作成
			// obj -> json -> {Config}Template.txt
			var text = BuildEmptyFormat();
			var path = m_template ? AssetDatabase.GetAssetPath(m_template)
				: AssetDatabase
					.GetAssetPath(this)
					.Replace(this.name, $"{m_targetObject.name}Template")
					.Replace(".asset", ".txt");

			File.WriteAllText(path, text);
			AssetDatabase.ImportAsset(path);

			m_template = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
		}

		protected abstract string BuildEmptyFormat();

		/// <summary>
		/// Configファイルを作る
		/// </summary>
		protected void CreateConfigObject()
		{
			// ScriptableObject
			var obj = CreateInstance();
			obj.name = obj.GetType().Name;
			var path = AssetDatabase.GetAssetPath(m_configDirectory) + $"/{obj.name}.asset";
			path = AssetDatabase.GenerateUniqueAssetPath(path);
			// Instantiate
			AssetDatabase.CreateAsset(obj, path);
			AssetDatabase.ImportAsset(path);

			m_targetObject = obj;
			EditorUtility.SetDirty(this);

			AssetDatabase.SaveAssets();
		}
		protected abstract ConfigObject CreateInstance();

		protected void Dump()
		{
			var obj = GetDeserializeObject();
			var str = EditorJsonUtility.ToJson(obj, true);
			Debug.Log(str);
		}
		protected abstract object GetDeserializeObject();
		#endregion
	}
}