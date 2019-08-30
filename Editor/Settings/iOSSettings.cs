using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// iOS用のビルド設定
	/// </summary>
	public sealed class iOSSettings : BuildSettings,
		IEnablePushNotification, IEnableRemoteNotification, IEnableGameCenter,
		IEnableBitCode
	{
		//============================
		//	const
		//============================

		//============================
		//	プロパティ
		//============================
		public override BuildTarget Target { get { return BuildTarget.iOS; } }
		public override BuildTargetGroup Group { get { return BuildTargetGroup.iOS; } }

		public bool EnableBitCode { get; }

		public bool EnablePushNotification => SystemCapability.EnablePushNotification;

		public bool EnableRemoteNotification => SystemCapability.EnableRemoteNotification;
		public bool EnableGameCenter => SystemCapability.EnableGameCenter;

		private iOSSystemCapability SystemCapability { get; set; }
		//============================
		//	関数
		//============================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public iOSSettings( 
			string			productName, 
			string			bundleId, 
			string			projectDirectoryName,
			string[]		includeScenes, 
			BuildOptions	buildOptions,
			iOSSystemCapability capability,
			bool bitCode = false
		)
			: base( productName, bundleId, projectDirectoryName, includeScenes, buildOptions )
		{
			SystemCapability = capability;
			EnableBitCode = bitCode;
		}
	}
}