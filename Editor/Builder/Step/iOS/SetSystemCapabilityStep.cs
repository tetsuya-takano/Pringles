using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using System;

namespace Pringles
{
	/// <summary>
	/// XCodeのシステム機能有効化設定
	/// </summary>
	public sealed class SetSystemCapabilityStep : XCodeProjectStep
	{
		//=========================================
		//	変数
		//=========================================
		private readonly bool               m_isEnable    = false;
		private readonly PBXCapabilityType  m_capability        = null;
		private readonly string             m_entitlementsPath = null;

		private string[] m_prijectTextLines = new string[ 0 ];
		//=========================================
		//	関数
		//=========================================
		public SetSystemCapabilityStep(string outputDir, PBXCapabilityType type, bool isEnable, string elementPath = null ) : base(outputDir)
		{
			m_isEnable = isEnable;
			m_capability = type;
			m_entitlementsPath = elementPath;
		}
		public SetSystemCapabilityStep(string outputDir, IEnableGameCenter gamecenter) : this(outputDir, PBXCapabilityType.GameCenter, gamecenter.EnableGameCenter) { }
		public SetSystemCapabilityStep(string outputDir, IEnablePushNotification pushNotification) : this(outputDir, PBXCapabilityType.PushNotifications, pushNotification.EnablePushNotification) { }

		protected override string Tag => "Set SystemCapability ";

		protected override BuildResult EditProject(string path, string targetName )
		{
			//	GameCenterなどのON/OFFパラメータが操作できないので
			//	全部見て対象の列を上書きする
			/*
			 ~~~~
			 例 )
			 com.apple.GameCenter.iOS = {
				enabled = 1;
			};
			~~~
			 */
			var allLines = File.ReadAllLines( path );
			var index = -1;
			for (int i = 0; i < allLines.Length; i++)
			{
				var line = allLines[ i ];
				if (line.Contains(m_capability.id))
				{
					//	Id行の次がパラメータなので + 1
					index = i + 1;
					break;
				}
			}
			if (index < 0 || index >= allLines.Length )
			{
				//	無いならセットできない
				//	とりあえず「あるやつの書き換え」だけなのでコレで
				return Warning($"{ m_capability.id } is Not Found");
			}
			if( index >= allLines.Length )
			{
				return Error($"File Length Over :{ index }:{ path }");
			}
			// 設定行を取得
			var settingsLine = allLines[ index ];
			// 書き換え
			var newLine = ReplaceEnableParam( settingsLine, m_isEnable );
			// 上書き
			allLines[ index ] = newLine;
			m_prijectTextLines = allLines;
			// ファイルに書き込み
			Log( path );
			return Success($"{m_capability.id} :: { newLine }");
		}

		protected override void SaveProject(string path)
		{
			File.WriteAllLines( path, m_prijectTextLines );
		}

		/// <summary>
		/// 対象のパラメータを書き換えた文字列を返す
		/// </summary>
		private string ReplaceEnableParam( string originLine, bool isEnable )
		{
			if (string.IsNullOrEmpty(originLine))
			{
				return originLine;
			}
			var list = originLine.ToCharArray();

			// \t\t\tenable = 1;

			// パラメータの位置を拾う
			// ; の1個前
			var valueIndex = Array.LastIndexOf(list, ';' ) - 1;
			if (valueIndex < 0)
			{
				//何もしない
				return originLine;
			}
			//	パラメータの位置
			var c = list.ElementAtOrDefault(valueIndex);
			var isInvalid = (c != '0' && c != '1') || c == char.MinValue;
			if ( isInvalid )
			{
				//	編集可能なモノでないので何もしない
				return originLine;
			}
			//	パラメータに合わせて入れ替え
			list[ valueIndex ] = isEnable ? '1' : '0';

			//	stringに戻す
			return new string( list );
		}
	}
}