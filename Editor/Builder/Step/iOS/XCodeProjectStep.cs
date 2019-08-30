using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

namespace Pringles
{
	/// <summary>
	/// XCodeプロジェクトを編集する処理
	/// </summary>
	public abstract class XCodeProjectStep : BuildStep
	{
		//=============================================
		//	変数
		//=============================================
		private readonly string m_projectDir = "";

		//=============================================
		//	プロパティ
		//=============================================
		protected string		BuildDirPath { get { return m_projectDir; } }
		protected PBXProject	Project { get; private set; }

		//=============================================
		//	関数
		//=============================================


		public XCodeProjectStep(string outputDir)
		{
			m_projectDir = outputDir;
		}
		protected override BuildResult DoExecute()
		{
			//	プロジェクトのパス
			
			var path = PBXProject.GetPBXProjectPath( m_projectDir );
			if (!File.Exists(path))
			{
				return Error($"Not Found XCodeProject : { path }");
			}
			var targetName = PBXProject.GetUnityTargetName();
			//	XCodeプロジェクトを読み込み
			Project = new PBXProject();
			Project.ReadFromFile( path );
			var result = EditProject( path, targetName );
			if ( result.Type == ResultType.Error )
			{
				return result;
			}
			SaveProject( path );
			return result;
		}

		protected abstract BuildResult EditProject(string path, string targetName );

		protected virtual void SaveProject( string path )
		{
			Project.WriteToFile(path);
		}
	}
	public abstract class XCodePlistStep : BuildStep
	{
		//=============================================
		//	const
		//=============================================
		private const string DefaultPlistName = "Info.plist";
		//=============================================
		//	変数
		//=============================================
		private readonly string m_projectDir = "";
		private readonly string m_plistName  = string.Empty;

		//=============================================
		//	プロパティ
		//=============================================
		protected string BuildDirPath { get { return m_projectDir; } }
		protected PlistDocument PlistDocument { get; private set; }

		//=============================================
		//	関数
		//=============================================


		public XCodePlistStep(string outputDir, string plistName)
		{
			m_projectDir = outputDir;
			m_plistName = plistName;
		}
		public XCodePlistStep(string outputDir) : this(outputDir, DefaultPlistName )
		{

		}

		protected override BuildResult DoExecute()
		{
			// pListのパス
			var path = Path.Combine(m_projectDir, m_plistName).ToUnityFilePath();
			if (!File.Exists(path))
			{
				return Error($"Not Found Plist : { path }");
			}
			//	Plistを読み込み
			PlistDocument = new PlistDocument();
			PlistDocument.ReadFromFile(path);
			var result = Edit( path );
			return result;
		}

		protected abstract BuildResult Edit( string path );

		protected virtual void SaveProject(string path)
		{
			PlistDocument.WriteToFile(path);
		}
	}
}
