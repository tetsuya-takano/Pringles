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
	/// StreamingAssets内のファイル一覧を作成する
	/// </summary>
	public sealed class StreamingAssetsFileListStep : BuildStep
	{
		[Serializable]
		private sealed class Table
		{
			[SerializeField]
			private List<string> m_list = new List<string>();
			public void Add( string path )
			{
				if( m_list.Contains( path ) )
				{
					PringlesUtils.Error( $"Same Key Add :{path}");
				}
				m_list.Add( path );
			}
		}
		//=======================================
		//	プロパティ
		//=======================================
		protected override string Tag { get { return "StreamingAssets FileList"; } }

		private readonly string m_outputFolder  = null;
		private readonly string m_fileName		= null;

		//=======================================
		//	関数
		//=======================================

		public StreamingAssetsFileListStep( string outputFolder, string fileName )
		{
			m_outputFolder	= outputFolder;
			m_fileName      = fileName;
		}

		protected override BuildResult DoExecute()
		{
			var allList         = AssetDatabase.GetAllAssetPaths();
			var targetDir       = "Assets/StreamingAssets/";
			var targetsFiles    = allList
									.Where( c => !AssetDatabase.IsValidFolder( c ) )//	フォルダ除外
									.Where( c => c.StartsWith( targetDir ) )		//	StreamingAssets以下
									.Select( c => c.Replace( targetDir,string.Empty ) )
									.ToArray();

			var table           = new Table();
			foreach( var file in targetsFiles )
			{
				table.Add( file );
			}
			var output          = Path.Combine( m_outputFolder, m_fileName );
			var json            = JsonUtility.ToJson( table, true  );
			if( string.IsNullOrEmpty( json ) )
			{
				return Error( "Json Parse Error." );
			}

			if( !Directory.Exists( m_outputFolder ) )
			{
				Directory.CreateDirectory( m_outputFolder );
			}
			File.WriteAllText( output, json, new UTF8Encoding( false ) );
			AssetDatabase.ImportAsset( output );
			return Success( $"{output} Count={targetsFiles.Length}" );
		}
	}
}