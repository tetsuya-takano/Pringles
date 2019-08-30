using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pringles;

namespace Pringles
{
	/// <summary>
	/// AndroidSdkを設定する
	/// </summary>
	public sealed class SetAndroidSDKPathStep : SetEnviromentValueStep
	{
		public SetAndroidSDKPathStep() : this("ANDROID_SDK_PATH") { }
		public SetAndroidSDKPathStep(string key) : base(key) { }

		protected override string Tag => "Set Android SDK";

		protected override BuildResult DoExecute()
		{
			var targetPreference = EditorPreference.ExternalTools.Android.SdkPath;
			var sdkPath = Get();

			targetPreference.Set( sdkPath );

			return Success($"{ targetPreference.Key} = {targetPreference.Value}");
		}
	}
	/// <summary>
	/// Android NDK
	/// </summary>
	public sealed class SetAndroidNDKPathStep : SetEnviromentValueStep
	{
		public SetAndroidNDKPathStep() : this("ANDROID_NDK_PATH") { }
		public SetAndroidNDKPathStep(string key) : base(key) { }

		protected override string Tag => "Set Android Ndk";

		protected override BuildResult DoExecute()
		{
			var targetPreference = EditorPreference.ExternalTools.Android.NdkPath;
			var sdkPath = Get();
			targetPreference.Set(sdkPath);

			return Success($"{ targetPreference.Key} = {targetPreference.Value}");
		}
	}
	/// <summary>
	/// Android JDK
	/// </summary>
	public sealed class SetAndroidJDKPathStep : SetEnviromentValueStep
	{
		public SetAndroidJDKPathStep() : this("ANDROID_JDK_PATH") { }
		public SetAndroidJDKPathStep(string key) : base(key) { }

		protected override string Tag => "Set Android Jdk";

		protected override BuildResult DoExecute()
		{
			var targetPreference = EditorPreference.ExternalTools.Android.JdkPath;
			var sdkPath = Get();

			targetPreference.Set(sdkPath);

			return Success($"{ targetPreference.Key} = {targetPreference.Value}");
		}
	}

	public sealed class SetAndroidJDKUseEmbeddedStep : BuildStep
	{
		private bool m_useEmbedded = true;
		protected override string Tag => "Set Android JdkUseEmbedded";

		public SetAndroidJDKUseEmbeddedStep(bool isEmbedded) { m_useEmbedded = isEmbedded; }
		protected override BuildResult DoExecute()
		{
			var targetPreference = EditorPreference.ExternalTools.Android.JdkUseEmbedded;
			targetPreference.Set( m_useEmbedded );

			return Success( $"{ targetPreference.Key }->{ targetPreference.Value }" );
		}
	}
}