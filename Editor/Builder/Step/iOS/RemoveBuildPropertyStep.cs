using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// ビルドプロパティを削除
	/// </summary>
	public class RemoveBuildPropertyStep : XCodeProjectStep
	{
		//=========================================
		// 変数
		//=========================================
		private readonly string m_propertyName = string.Empty;

		//=========================================
		// 関数
		//=========================================
		public RemoveBuildPropertyStep(string outputDir,string propertyName) : base(outputDir)
		{
			m_propertyName = propertyName;
		}

		protected override string Tag => "Remove BuildProperty";

		protected override BuildResult EditProject(string path, string targetName)
		{
			var guid = Project.TargetGuidByName( targetName );
			Project.SetBuildProperty(guid, m_propertyName, string.Empty);

			return Success(Project.GetBuildPropertyForAnyConfig(guid, m_propertyName));
		}
	}
}