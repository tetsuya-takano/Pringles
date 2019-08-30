using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pringles
{
	public abstract class TemplateConverter<T> : TemplateConverter
		where T : ReplaceFormat
	{
		public T Format => GetFormat<T>();
	}
	/// <summary>
	/// テンプレートを書き出すファイル
	/// </summary>
	public abstract class TemplateConverter : ScriptableObject
	{
		[Tooltip("テンプレート")]
		[SerializeField] private TemplateLocater m_locater = default;
		[Tooltip("変更ルール")]
		[SerializeField] private ReplaceFormat m_rule = default;

		protected T GetFormat<T>() where T : ReplaceFormat
		{ 
			return m_rule as T;
		}

		/// <summary>
		/// 実行
		/// </summary>
		[ContextMenu(nameof(Build))]
		public void Build()
		{
			// テンプレートのファイルを持ってくる
			var files = m_locater.GetTemplates();
			// 変換結果出力
			foreach( var template in files )
			{
				var text = File.ReadAllText( template.AbsolutePath );
				// テンプレートの内容を差し替え
				var content = m_rule.Replace( text );

				// 差し替え先取得
				var path = m_locater.GetOriginal( template ).AbsolutePath;
				if (!File.Exists(path))
				{
					throw new FileNotFoundException(path);
				}
				// 置き換え
				File.WriteAllText(path, content);
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public static T Builder<T>() where T : TemplateConverter
		{
			var path = AssetDatabase
				.FindAssets($"t:{typeof(T).Name}")
				.Select(guid => AssetDatabase.GUIDToAssetPath(guid))
				.FirstOrDefault();

			return AssetDatabase.LoadAssetAtPath<T>(path);
		}
	}
}