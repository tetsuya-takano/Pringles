using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// Resourcesのファイル一覧テーブルを作成する処理
	/// </summary>
	public class ResourcesFileListBuildStep : BuildStep
	{
		[Serializable]
		private sealed class Table
		{
			[SerializeField] private List<string> m_list = new List<string>();
			public void Add( string path )
			{
				if( m_list.Contains( path ))
				{
					PringlesUtils.Error( $"Same Key Add :{path}" );
				}
				m_list.Add( path );
			}
		}
		//============================================
		//	変数
		//============================================
		private readonly string m_folder	= null;
		private readonly string m_fileName	= null;

		//============================================
		//	プロパティ
		//============================================
		protected override string Tag { get { return "Build Resources FileList"; } }

		//============================================
		//	関数
		//============================================

		public ResourcesFileListBuildStep( 
			string outputFolder,
			string fileName
		)
		{
			m_folder    = outputFolder;
			m_fileName  = fileName;
		}

		protected override BuildResult DoExecute()
		{
			var allList			= AssetDatabase.GetAllAssetPaths();
			var targetsFiles	= allList
									.Where( c => !AssetDatabase.IsValidFolder( c ) )	//	フォルダ除外
									.Where( c => !c.Contains( "/Editor/" ))			//	Editor除外
									.Where( c =>  c.Contains( "/Resources/" ))		//	Resourcesのみ
									.ToArray();

			var table           = new Table();
			var regex           = new Regex( "(.*?)/Resources/(.*?)" );

			foreach( var f in targetsFiles )
			{
				var match = regex.Match( f );
				if( !match.Success )
				{
					continue;
				}

				//	XXXXXX/Resources/ の部分を切る
				var prefex = match.Groups[ 0 ].Value;

				table.Add( f.Replace( prefex, string.Empty ) );
			}

			var output          = Path.Combine( m_folder, m_fileName );
			var json            = JsonUtility.ToJson( table, true  );
			if( string.IsNullOrEmpty( json ) )
			{
				return Error( "Json Parse Error." );
			}

			if( !Directory.Exists( m_folder) )
			{
				Directory.CreateDirectory( m_folder );
			}
			File.WriteAllText( output, json, new UTF8Encoding( false ) );
			AssetDatabase.ImportAsset( output );
			return Success( $"{output} Count={targetsFiles.Length}" );
		}
	}
}