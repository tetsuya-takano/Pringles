using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

namespace Pringles
{
	/// <summary>
	/// ビルド設定ファイルを作成するウィザード
	/// </summary>
	public sealed class SettingsBuildWindow : ScriptableWizard
	{
		//==================================
		// class
		//==================================
		[Serializable]
		private sealed class SceneData
		{
			public bool enable;
			public string guid;
			public string path;
		}
		[CustomPropertyDrawer(typeof(SceneData))]
		private sealed class SceneDataDrawer : PropertyDrawer
		{
			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
				var p_enable = property.FindPropertyRelative($"enable");
				var p_guid = property.FindPropertyRelative($"guid");
				var r_enable = new Rect(position);
				var r_scene = new Rect(position);
				r_enable.width *= 0.3f;
				r_scene.width = position.width - r_enable.width;
				r_scene.xMin = r_enable.xMax;
				EditorGUI.PropertyField(r_enable, p_enable);
				var scenePath = AssetDatabase.GUIDToAssetPath(p_guid.stringValue);
				var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>( scenePath );
				EditorGUI.ObjectField(r_scene, asset, typeof(SceneAsset), allowSceneObjects: false);
			}
		}

		//==================================
		// SerializeField
		//==================================
		[SerializeField] private string m_settingsName = string.Empty;

		[Header("Settings")]
		[SerializeField] private string m_productName;
		[SerializeField] private string m_bundleId;

		[Header("Options")]
		[SerializeField] private bool m_isDevelopment = true;
		[SerializeField] private bool m_isProfiler = true;
		[SerializeField] private bool m_isDebuging = true;

		[Header("Scenes")]
		[SerializeField]
		private SceneData[] m_sceneList = new SceneData[0];

		//==================================
		// 変数
		//==================================

		//==================================
		// 関数
		//==================================

		[MenuItem("Tools/Pringles/Settings Wizard")]
		static void Open()
		{
			ScriptableWizard.DisplayWizard<SettingsBuildWindow>("Settings Build", "Create", "Read");
		}
		private void OnEnable()
		{
			RefreshSceneList();
		}
		protected override bool DrawWizardGUI()
		{
			DrawOption();
			return base.DrawWizardGUI();
		}
		/// <summary>
		/// 作成
		/// </summary>
		private void OnWizardCreate()
		{
			if( !SaveFiles( out var resultPath ))
			{
				return;
			}
			Build( resultPath );
		}

		private bool SaveFiles( out string resultPath )
		{
			var result = EditorUtility.SaveFolderPanel( "Save", Application.dataPath, m_settingsName );
			if (string.IsNullOrEmpty(result))
			{
				resultPath = string.Empty;
				return false;
			}
			resultPath = result;
			return true;
		}
		private void Build( string directory )
		{
			var dir = Path.Combine(directory, m_settingsName);
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
			var scenes = m_sceneList.Where(c => c.enable).Select(c => c.guid).ToArray();
			var sceneJson = BuildSceneJson.Create(scenes);
			SaveJson(dir, $"scenes", sceneJson);

			var buildJson = AppBuildSettingsJson.Create(m_isDevelopment, m_isDebuging, m_isProfiler);
			SaveJson(dir, $"option", buildJson);

			var identifierJson = RomIdentifierJson.Create(m_productName, m_bundleId);
			SaveJson(dir, $"identifier", identifierJson);
		}
		private void SaveJson( string directory, string fileName, object json)
		{
			var path = $"{directory}/{fileName}.json";
			var contents = JsonUtility.ToJson(json, true);
			File.WriteAllText(path, contents);
		}

		private void RefreshSceneList()
		{
			m_sceneList = EditorBuildSettings
				.scenes
				.Select(s => new SceneData
				{
					enable = s.enabled,
					guid = s.guid.ToString(),
					path = s.path
				})
				.OrderByDescending(c => c.enable)
				.ThenByDescending(c => c.path)
				.ToArray();
		}

		private void RefreshBuildSettings()
		{
			m_isDevelopment = EditorUserBuildSettings.development;
			m_isDebuging = EditorUserBuildSettings.allowDebugging;
			m_isProfiler = EditorUserBuildSettings.compressFilesInPackage;
		}
		private void RefreshProductInfo()
		{
			m_bundleId = PlayerSettings.applicationIdentifier;
			m_productName = PlayerSettings.productName;
		}

		private void DrawOption()
		{
			using( var vertical= new GUILayout.VerticalScope())
			{
				using (var horizon = new GUILayout.HorizontalScope())
				{
					if (GUILayout.Button("Reload Build Option"))
					{
						RefreshBuildSettings();
					}
					if (GUILayout.Button("Reload Product Info"))
					{
						RefreshProductInfo();
					}
					if (GUILayout.Button("Reload BuildSettings Scene"))
					{
						RefreshSceneList();
					}
				}
				EditorGUILayout.Space();
			}
		}
	}
}