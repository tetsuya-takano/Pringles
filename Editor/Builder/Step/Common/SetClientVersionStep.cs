using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pringles
{
	public sealed class SetClientVersionStep : BuildStep
	{
		//========================
		// 変数
		//========================
		private readonly string m_version = null;

		//========================
		// プロパティ
		//========================
		protected override string Tag => "Set Version";

		//========================
		// 関数
		//========================
		public SetClientVersionStep(string version)
		{
			m_version = version;
		}

		protected override BuildResult DoExecute()
		{
			Version v = null;
			if (!Version.TryParse(m_version, out v))
			{
				return Error($"Version Format Error : {m_version}");
			}
			var current = PlayerSettings.bundleVersion;
			var next = v.ToString();

			PlayerSettings.bundleVersion = next;

			return Success($"Set Vetsion {current} -> {next}");
		}
	}
}