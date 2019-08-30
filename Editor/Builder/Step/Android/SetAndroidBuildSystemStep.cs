using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// ビルドシステムの変更
	/// </summary>
	public class SetAndroidBuildSystemStep : BuildStep
	{
		protected override string Tag { get { return "Set Android BuildSystem"; } }
		//========================================
		//	変数
		//========================================
		private readonly AndroidBuildSystem m_system = AndroidBuildSystem.Gradle;

		//========================================
		//	関数
		//========================================

		public SetAndroidBuildSystemStep( AndroidBuildSystem system )
		{
			m_system = system;
		}

		protected override BuildResult DoExecute()
		{
			EditorUserBuildSettings.androidBuildSystem = m_system;
			return Success( m_system.ToString() );
		}
	}
}