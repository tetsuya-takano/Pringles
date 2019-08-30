using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pringles
{
	/// <summary>
	/// 指定ファイルのコピー
	/// </summary>
	public sealed class CopyFileStep : BuildStep
	{
		//================================================
		//	enum
		//================================================
		public enum Option
		{
			None	,	//	上書きする
			Warning	,	//	警告を出して進む
			Error	,	//	エラーを出す
			DryRun, // なにもしない(ログ用)
		}

		//================================================
		//	プロパティ
		//================================================
		protected override string Tag { get { return "Copy File"; } }

		//================================================
		//	変数
		//================================================
		private readonly string m_sourceDirPath = null;
		private readonly string m_targetDirPath = null;
		private readonly IMatchRule m_fileRule = null;
		private readonly IMatchRule m_filterRule = null;
		private readonly Option  m_overWriteMode		= Option.Error;

		//================================================
		//	関数
		//================================================
		public static CopyFileStep FromPatternInput( string sourceDirPath, string targetDirPath, string pattern, Option mode )
		{
			return new CopyFileStep(
				sourceDirPath, 
				targetDirPath, 
				new RegexMatchRule(pattern), 
				new ThroughRule(), 
				mode
			);
		}
        public static CopyFileStep FromPatternInput(string sourceDirPath, string targetDirPath, string pattern, string[] ignoreList, Option mode)
        {
			return new CopyFileStep(
				sourceDirPath, 
				targetDirPath, 
				new RegexMatchRule(pattern), 
				new RegexMatchRule(ignoreList).Reverce(), 
				mode
			);
		}
        public static CopyFileStep FromPatternFile( string sourceDirPath, string targetDirPath, string pattern, string ignoreFilePath, Option mode )
		{
			return new CopyFileStep(
				sourceDirPath, 
				targetDirPath, 
				new RegexMatchRule(pattern), 
				new RegexMatchRule(File.ReadAllLines(ignoreFilePath)).Reverce(), 
				mode
			);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CopyFileStep(string sourceDirPath, string targetDirPath, IMatchRule fileRule, IMatchRule filterRule, Option mode)
		{
			m_sourceDirPath = sourceDirPath.ToUnityFilePath();
			m_targetDirPath = targetDirPath.ToUnityFilePath();
			m_fileRule = fileRule;
			m_filterRule = filterRule;
			m_overWriteMode = mode;
		}
		
		/// <summary>
		/// 実行
		/// </summary>
		protected override BuildResult DoExecute()
		{
			if( !Directory.Exists( m_sourceDirPath ) )
			{
				return Warning( string.Format( "Directory Not Found : {0}", m_sourceDirPath ) );
			}
			Log($"[{m_sourceDirPath}] -> [{m_targetDirPath}] File:{m_fileRule?.ToString()} / Filter:{m_filterRule?.ToString()}");

			var targetFiles    = Directory
									.EnumerateFiles( m_sourceDirPath,"*", SearchOption.AllDirectories )
									.Where( f =>  m_fileRule.IsMatch( f ))
									.Select( c => c.ToUnityFilePath())
									.ToArray();
			var sourceFiles		= targetFiles
									.Where(f => m_filterRule.IsMatch(f))
									.ToArray();
			//	基ディレクトリがなかったら作成
			if( !Directory.Exists( m_targetDirPath ) )
			{
				Directory.CreateDirectory( m_targetDirPath );
			}
			if( m_overWriteMode == Option.DryRun)
			{
				return LogOnly( sourceFiles );
			}

			return Copy( sourceFiles );
		}
		/// <summary>
		/// コピーあり
		/// </summary>
		private BuildResult Copy( IEnumerable<string> sourceFiles )
		{
			var builder         = new StringBuilder();
			var isOverWriteAny  = false;
			var isOverWrite     = m_overWriteMode != Option.Error;  //	上書きはするかどうか
			foreach( var f in sourceFiles )
			{
				var destFilePath = f.Replace( m_sourceDirPath, m_targetDirPath );
				if( File.Exists( destFilePath ) )
				{
					if( m_overWriteMode == Option.Error )
					{
						//	上書き不可はエラー終了
						return Error( $"Can't Over Write : {f} -> {destFilePath}");
					}
					if( isOverWrite )
					{
						//	上書き可は ログに積む
						isOverWriteAny = true;
						builder
							.AppendFormat( "{0} -> {1}", f, destFilePath )
							.AppendLine();
					}
				}
				var parentDirPath = Path.GetDirectoryName( destFilePath );
				if( !Directory.Exists( parentDirPath ) )
				{
					Directory.CreateDirectory( parentDirPath );
				}
				File.Copy( f, destFilePath, isOverWrite );
			}
			if( isOverWrite && isOverWriteAny )
			{
				//	警告が必要なモードはログを出す
				return Warning( $"Over Write File :{builder.ToString()}");
			}
			//	完了
			return Success( $"Copy Directory : {m_sourceDirPath}");
		} 

		/// <summary>
		/// ログのみ
		/// </summary>
		private BuildResult LogOnly( IEnumerable<string> sourceFiles )
		{
			var builder = new StringBuilder();
			builder.AppendLine();
			foreach( var p in sourceFiles )
			{
				builder.AppendLine( p.Replace( m_sourceDirPath, string.Empty ) );
			}

			return Success( $"Copy Target : {builder.ToString()}" );
		}
	}
}