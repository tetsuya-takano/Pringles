using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Pringles
{
    public abstract class DeleteStep : BuildStep
    {
		public enum Option
		{
			Warning,
			None,
			DryRun,
		}
    }

	/// <summary>
	/// 指定ファイルの削除
	/// </summary>
	public sealed class DeleteFileStep : DeleteStep
	{
		//================================================
		//	プロパティ
		//================================================
		protected override string Tag { get { return "Delete File"; } }

		//================================================
		//	変数
		//================================================
		private readonly string m_directoryPath = null;
		private readonly string m_filePattern	= null;
		private readonly Option m_option = Option.Warning;

		//================================================
		//	関数
		//================================================

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public DeleteFileStep( string directoryPath, string pattern, Option option = Option.Warning )
		{
			m_directoryPath = directoryPath;
			m_filePattern   = pattern;
			m_option = option;
		}

		/// <summary>
		/// 実行
		/// </summary>
		protected override BuildResult DoExecute()
		{
			if( !Directory.Exists( m_directoryPath ) )
			{
				return Warning( $"Directory Not Found : {m_directoryPath}" );
			}

			var directory = new DirectoryInfo( m_directoryPath );
			var files	  = directory.GetFiles( m_filePattern, SearchOption.AllDirectories );

			//	削除
			foreach( var f in files )
			{
				switch (m_option)
				{
					case Option.DryRun:
						Log(f.FullName);
						continue;
					case Option.Warning:
						Warning(f.FullName);
						break;
				}
				f.Delete();
			}

			//	完了
			return Success( $"Delete Directory : {m_directoryPath}");
		}
	}

	/// <summary>
	/// ディレクトリの削除
	/// </summary>
	public sealed class DeleteDirectoryStep : DeleteStep
    {
		protected override string Tag { get { return "Delete Directory"; } }

		//=====================================
		//	変数
		//=====================================
		private readonly string m_dirPath = null;
		private readonly Option m_optioin = Option.Warning;
		//=====================================
		//	関数
		//=====================================
		/// <summary>
		/// 
		/// </summary>
		public DeleteDirectoryStep( string directoryPath, Option option = Option.Warning )
		{
			m_dirPath = directoryPath;
			m_optioin = option;
		}

		protected override BuildResult DoExecute()
		{
			if( !Directory.Exists( m_dirPath ) )
			{
				return Warning( $"Directory Not Found. {m_dirPath}");
			}
			if (m_optioin != Option.DryRun)
			{
				Directory.Delete(m_dirPath, true);
			}
			return Success( $"Directory Delete. {m_dirPath}" );
		}
	}
}