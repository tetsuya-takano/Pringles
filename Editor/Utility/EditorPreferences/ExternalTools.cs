using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Pringles
{
	/// <summary>
	/// ExternalToolsを触る
	/// </summary>
	public static partial class EditorPreference
	{
		public static class ExternalTools
		{
			/// <summary>
			/// Androidの値
			/// </summary>
			public static class Android
			{
				//===========================
				// const
				//===========================
				private const string PREFS_KEY_SDK_PATH = "AndroidSdkRoot";
				private const string PREFS_KEY_JDK_PATH = "JdkPath";
				private const string RPREFS_KEY_NDK_PATH =
#if UNITY_2018_3_OR_NEWER
					"AndroidNdkRootR16b"
#else
					"AndroidNdkRoot"
#endif
				;
				private const string PREFS_KEY_JDK_USE_EMBEDDED = "JdkUseEmbedded";

				//===========================
				// プロパティ
				//===========================
				public static PrefsData<string> SdkPath { get; } = new StringPrefsData(PREFS_KEY_SDK_PATH);
				public static PrefsData<string> NdkPath { get; } = new StringPrefsData(RPREFS_KEY_NDK_PATH);
				public static PrefsData<string> JdkPath { get; } = new StringPrefsData(PREFS_KEY_JDK_PATH);

				public static PrefsData<bool> JdkUseEmbedded { get; } = new BoolPrefsData(PREFS_KEY_JDK_PATH);
			}
		}
	}
}