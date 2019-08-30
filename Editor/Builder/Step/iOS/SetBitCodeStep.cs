using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using UnityEngine;
namespace Pringles
{
	/// <summary>
	/// XCodeのシステム機能有効化設定
	/// </summary>
	public sealed class SetBitCodeStep : XCodeProjectStep
	{
		//=========================================
		//	const
		//=========================================
		private const string BITCODE_PROPERTY = "ENABLE_BITCODE";

		//=========================================
		//	変数
		//=========================================
		private readonly bool m_isEnable = false;

		//=========================================
		//	関数
		//=========================================
		public SetBitCodeStep(string outputDir, bool isEnable) : base(outputDir)
		{
			m_isEnable = isEnable;
		}
		public SetBitCodeStep(string outputDir, IEnableBitCode bitcode) : this(outputDir, bitcode.EnableBitCode) { }

		protected override string Tag => "Set BitCode";

		protected override BuildResult EditProject(string path, string targetName )
		{
			var guid = Project.TargetGuidByName( targetName );
			Project.SetBuildProperty(guid, BITCODE_PROPERTY, m_isEnable ? "YES" : "NO");

			return Success($"{Project.GetBuildPropertyForConfig(guid, BITCODE_PROPERTY) }");
		}
	}
}