using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pringles
{
	/// <summary>
	/// ビルド番号の設定
	/// </summary>
	public sealed class SetBuildNumberStep : BuildStep
	{
		//====================================
		//	変数
		//====================================
		private readonly int m_number = -1;

		//====================================
		//	プロパティ
		//====================================
		protected override string Tag { get { return "[Set Build Number]"; } }

		//====================================
		//	関数
		//====================================

		/// <summary>
		/// 
		/// </summary>
		public SetBuildNumberStep( int number )
		{
			m_number = number;
		}

		/// <summary>
		/// 実行
		/// </summary>
		protected override BuildResult DoExecute()
		{
			SetBuildNumber( m_number );
			return Success( $"Build Number = {m_number}" );
		}

		/// <summary>
		/// ビルド番号の設定
		/// </summary>
		private void SetBuildNumber( int number )
		{
			NumberSetter.Set( number );
		}

		private abstract class NumberSetter
		{
			public static void Set( int num )
			{
				NumberSetter setter = null;
#if UNITY_IOS
				setter = new IOSSetter();
#elif UNITY_ANDROID
				setter = new AndroidSetter();
#else
				setter = new MiscSetter();
#endif
				setter.DoSet( num );
			}
			protected abstract void DoSet( int num );
		}
#if UNITY_IOS
		private class IOSSetter : NumberSetter
		{
			protected override void DoSet( int num ) => PlayerSettings.iOS.buildNumber = num.ToString();
		}
#elif UNITY_ANDROID
		private class AndroidSetter : NumberSetter
		{
			protected override void DoSet( int num ) => PlayerSettings.Android.bundleVersionCode = num;
		}
#else
		private class MiscSetter : NumberSetter
		{
			protected override void DoSet( int num ) => Debug.LogWarning( $"Not Supported Platform : Num = {num}" );
		}
#endif
	}
}