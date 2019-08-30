using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build.Player;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

namespace Pringles
{
	/// <summary>
	/// コンパイルする
	/// </summary>
	public sealed class ScriptCompileStep : BuildStep
	{
		//=======================================
		// プロパティ
		//=======================================
		protected override string Tag => $"Script Compile :{Group} in {Target}";
		private BuildTargetGroup Group { get; }
		public bool IgnoreClean { get; }
		public string ExportDir { get; }
		private BuildTarget Target { get;  }

		//=======================================
		// 関数
		//=======================================

		public ScriptCompileStep(string outputDir, BuildTarget target, BuildTargetGroup group, bool ignoreClean = false )
		{
			ExportDir = outputDir;
			Target = target;
			Group = group;
			IgnoreClean = ignoreClean;
		}
		protected override BuildResult DoExecute()
		{
			// refresh builded assemblies
			if (!Directory.Exists(ExportDir))
			{
				Directory.CreateDirectory(ExportDir);
			}
			var settings = new ScriptCompilationSettings
			{
				group = Group,
				options = ScriptCompilationOptions.DevelopmentBuild,
				target = Target
			};
			var result = PlayerBuildInterface.CompilePlayerScripts(settings, ExportDir);

			if( !IgnoreClean )
			{
				// refresh builded assemblies
				if (Directory.Exists(ExportDir))
				{
					Directory.Delete(ExportDir, true);
				}
			}

			var assemblies = result.assemblies;
			var typeDB = result.typeDB;
			var isSuccess = assemblies != null && assemblies.Count != 0 && typeDB != null;
			if( isSuccess)
			{
				return Success($"{string.Join(",", assemblies)}");
			}
			if( assemblies == null)
			{
				return Error($"assemblies is Null");
			}
			if (typeDB == null)
			{
				return Error($"typeDB is Null");
			}
			return Error("assemblies is Empty");
		}
	}
}
