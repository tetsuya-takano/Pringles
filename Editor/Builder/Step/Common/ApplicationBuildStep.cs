using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Pringles
{
	public class ApplicationBuildStep : BuildStep
	{
		//==============================
		//	プロパティ
		//==============================
		protected override string Tag { get { return "Build Rom"; } }

		//==============================
		//	変数
		//==============================
		private readonly string				m_bundleId	 = null;
		private readonly string				m_productName= null;
		private readonly string				m_outputPath = null;
		private readonly BuildTargetGroup   m_group      = BuildTargetGroup.Unknown;
		private readonly BuildTarget		m_buildTarget= BuildTarget.NoTarget;
		private readonly BuildOptions		m_options    = BuildOptions.None;
		private readonly string[]			m_levels     = null;

		//==============================
		//	関数
		//==============================
		public ApplicationBuildStep(
			string				productName,
			string				bundleId,
			string[]			levels, 
			string				outputPath, 
			BuildTargetGroup	group,
			BuildTarget			target, 
			BuildOptions		options )
		{
			m_productName= productName;
			m_bundleId   = bundleId;

			m_levels	 = levels;
			m_outputPath = outputPath;
			m_group      = group;
			m_buildTarget= target;
			m_options	 = options;
		}

		/// <summary>
		/// BuildSettingsによるビルド
		/// </summary>
		public ApplicationBuildStep(IBuildSettings settings) : this
			(
			build:settings,
			productName: settings,
			bundleId: settings,
			levels: settings,
			option:settings,
			location:settings
			) { }
		public ApplicationBuildStep( 
			IBuildTarget build,
			IProductName productName,
			IBundleId bundleId, 
			IBuildScenes levels,
			IBuildOption option,
			ILocationPath location) 
			: this
		(
				  productName: productName.ProductName,
				  bundleId:bundleId.BundleId,
				  levels:levels.Levels,
				  outputPath:location.LocationPath,
				  group:build.Group,
				  target: build.Target,
				  options:option.Option
		) { }

		protected override BuildResult DoExecute()
		{
			PlayerSettings.SetApplicationIdentifier( m_group, m_bundleId );
			PlayerSettings.productName = m_productName;


			Debug.Log($"BundleId = {PlayerSettings.GetApplicationIdentifier(m_group)}");
			Debug.Log($"ProductName = {PlayerSettings.productName}");

			var report = BuildPipeline.BuildPlayer
							(
								levels				: m_levels,
								locationPathName	: m_outputPath,
								target				: m_buildTarget,
								options				: m_options
							);
			var isSuccess = false;
#if UNITY_2018_1_OR_NEWER
			isSuccess = report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded;
#else
			isSuccess = report.IsNullOrEmpty();
#endif
			if( isSuccess )
			{
				return Success( $"Rom Build Completed... => {m_outputPath}\n{ToReportLog( report )}" );
			}

			return Error( $"Rom Build Error : {ToReportLog( report )}");
		}
#if UNITY_2018_1_OR_NEWER
		private string ToReportLog( BuildReport report )
		{
			var builder = new StringBuilder();
			var summary = report.summary;
			builder
				.AppendLine( "■■■■■■■[Summary]■■■■■■" )
				.AppendLine( $"Platform : {summary.platform}" )
				.AppendLine( $"Result : {summary.result}" )
				.AppendLine( $"Guid : {summary.guid.ToString()}" )
				.AppendLine( $"TotalSize Size:{summary.totalSize/1024/1024}MB" )
				.AppendLine( $"Error Count:{summary.totalErrors}" )
				.AppendLine( $"Warning Count:{summary.totalWarnings}" )
				;

			//	内包リソースサイズの出力
			builder.AppendLine( $"StreamingAssetsSize : { GetDirSizeStr( Application.streamingAssetsPath )}" );

			var steps = report.steps;
			builder.AppendLine( "■■■■■■■[Build Step]■■■■■■■" );
			foreach( var step in steps )
			{
				builder
					.Append( $"Name : [{step.name}],")
					.Append( $"Depth : [{step.depth}],")
					.Append( $"Duration : [{step.duration.ToString( )}]" )
					.AppendLine()
					;
				var errors = step
								.messages
								.Where( c => c.type == LogType.Error || c.type == LogType.Assert || c.type == LogType.Exception )
								.ToArray();
				builder.Append( "\t" ).AppendLine( "[Error]" );
				foreach( var msg in errors )
				{
					builder.Append( "\t" ).AppendLine( $" -- { msg.content }" );
				}
			}

			return builder.ToString();
		}
#else
		private string ToReportLog( string report )
		{
			return report;
		}
#endif

		/// <summary>
		/// ディレクトリサイズを取得
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private long GetDirSize( string path )
		{
			if( !Directory.Exists( path ) )
			{
				return -1;
			}
			var info = new DirectoryInfo( path );
			long sum  = 0;
			foreach( var f in info.EnumerateFiles( "*", SearchOption.AllDirectories ) )
			{
				sum += f.Length;
			}
			return sum;
		}

		/// <summary>
		/// 
		/// </summary>
		private string GetDirSizeStr( string path )
		{
			var bytes = GetDirSize( path );
			if( 0 < bytes )
			{
				return $"Size Error:{ bytes }";
			}
			// B
			if( bytes < 1024 )
			{
				return $"{bytes}B";
			}
			//	KB
			var kb = bytes / 1024;
			if( kb < ( 1024 ) )
			{
				return $"{kb}KB({bytes}B)";
			}
			//	MB
			var mb = kb / 1024;
			if( mb < ( 1024 ) )
			{
				return $"{mb}MB({kb}KB)";
			}
			// GB
			var gb = mb / 1024;
			return $"{gb}GB({mb}MB)";
		}
	}
}