using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// Windowsビルド用
	/// </summary>
	public class WinSettings : BuildSettings
	{
		public WinSettings(
			string productName, 
			string bundleId, 
			string locationPath, 
			string[] includeScenes, 
			BuildOptions buildOptions) 
			: base(productName, bundleId, locationPath, includeScenes, buildOptions)
		{
		}

		public override BuildTarget Target => BuildTarget.StandaloneWindows64;

		public override BuildTargetGroup Group => BuildTargetGroup.Standalone;
	}
}