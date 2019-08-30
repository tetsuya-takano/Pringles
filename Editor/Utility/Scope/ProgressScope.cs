using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// 進捗バー表示
	/// </summary>
	public sealed class ProgressScope : IDisposable
	{
		//================================
		// 変数
		//================================
		private string m_title = string.Empty;
		//================================
		// 関数
		//================================
		public ProgressScope( string title )
		{
			m_title = title;
		}
		public void Show( string msg, float progress )
		{
			EditorUtility.DisplayProgressBar( m_title, msg, progress );
		}
		public void Dispose()
		{
			EditorUtility.ClearProgressBar();
		}
	}
}