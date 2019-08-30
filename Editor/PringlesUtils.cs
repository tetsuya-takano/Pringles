using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Pringles
{
	/// <summary>
	/// ユーティリティ
	/// </summary>
	public static class PringlesUtils
	{
		//====================================
		//	const
		//====================================
		private const string HEADER = "[Pringles] {0}";

		//====================================
		//	static Property
		//====================================
		public static bool IsBatchMode
		{
#if UNITY_2018_3_OR_NEWER
			get { return Application.isBatchMode; }
#else
			get { return System.Environment.CommandLine.Contains( "-batchmode" ); }
#endif
		}

		//====================================
		//	Method
		//====================================

		/// <summary>
		/// 
		/// </summary>
		public static void Log( string msg )
		{
			Debug.LogFormat( HEADER, msg );
		}
		/// <summary>
		/// 
		/// </summary>
		public static void Warning( string msg )
		{
			Debug.LogWarningFormat( HEADER, msg );
		}

		/// <summary>
		/// 
		/// </summary>
		public static void Error( string msg )
		{
			Debug.LogErrorFormat( HEADER, msg );
		}

		/// <summary>
		/// ビルドの基盤処理
		/// </summary>
		public static void Build( this BuildPipelineBuilder builder )
		{
			var result = builder.Run();
			switch( result.Type )
			{
				case ResultType.Success:
					Log( result.ToString() );
					break;
				case ResultType.Warning:
					Warning( result.ToString() );
					break;
				case ResultType.Error:
					Error( result.ToString() );
					break;
			}
			if( !IsBatchMode )
			{
				return;
			}
			//	Error BatchMode return Exit Code
			if( !( result.Type == ResultType.Error ) )
			{
				return;
			}
			EditorApplication.Exit( 1 );
		}

		public static bool IsError( this BuildResult self)
		{
			return self.Type == ResultType.Error;
		}
		public static bool IsSuccessOrWarning(this BuildResult self)
		{
			return !self.IsError();
		}

		/// <summary>
		/// 
		/// </summary>
		public static string ToUnityFilePath(this string self)
		{
			return self.Replace( "\\", "/" );
		}
        /// <summary>
        /// Application.dataPath / Application.streamingAssetsPath を実際にものに差し替える
        /// </summary>
        public static string ToReplaceDefinedPath(string path)
        {
            return path
                .Replace(@"\$\{Application.dataPath\}", Application.dataPath)
                .Replace(@"\$\{Application.streamingAssetsPath\}", Application.streamingAssetsPath)
                ;
        }
    }
}