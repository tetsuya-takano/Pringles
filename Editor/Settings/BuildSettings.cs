using System;
using System.Text;
using UnityEditor;
using Pringles.Parser;

namespace Pringles
{

	public interface IBuildSettings 
		: IBuildOption,
		  IBundleId,
		  IBuildTarget,
		  ILocationPath,
		  IProductName,
		  IBuildScenes
	{
	}
	/// <summary>
	/// ビルドの設定
	/// </summary>
	public abstract class BuildSettings : IBuildSettings
	{
		//============================================
		//! メンバー変数
		//============================================
		public abstract BuildTarget Target { get; }
		public abstract BuildTargetGroup Group { get; }

		public BuildOptions Option { get; private set; }

		public string ProductName { get; private set; }
		public string BundleId { get; private set; }

		public string LocationPath { get; private set; }

		public string[] Levels { get; private set; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public BuildSettings(
			string				productName		,
			string				bundleId		,
			string				locationPath	,
			string[]			includeScenes	,
			BuildOptions		buildOptions	
		)
		{
			ProductName			= productName		;
			BundleId			= bundleId			;
			LocationPath		= locationPath		;
			Levels		= includeScenes		;
			Option		= buildOptions		;
		}

		//--------------------------------------------
		// public
		//--------------------------------------------
		/// <summary>
		/// ビルド設定情報を文字列に変換します
		/// </summary>
		public string toString()
		{
			var builder = new StringBuilder();
			builder
				.Append( "BuildTargetGroup	：" ).AppendLine( Group.ToString() )
				.Append( "BuildTarget		：" ).AppendLine( Target.ToString() )
				.Append( "ProductName		：" ).AppendLine( ProductName.ToString() )
				.Append( "BundleId			：" ).AppendLine( BundleId.ToString() )
				.Append( "LocationPath		：" ).AppendLine( LocationPath.ToString() )
				;
			return builder.ToString();
		}
	}
}