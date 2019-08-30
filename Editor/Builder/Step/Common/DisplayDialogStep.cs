using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// ダイアログを表示する
	/// </summary>
	public sealed class DisplayDialogStep : BuildStep
	{
		//==========================================
		//	プロパティ
		//==========================================
		protected override string Tag { get { return $"Dialog : {m_dialogName}"; } }

		//==========================================
		//	変数
		//==========================================
		private readonly string m_dialogName		= string.Empty;
		private readonly string m_displayMessage	= string.Empty;
		private readonly string m_okText			= string.Empty;
		private readonly string m_cancelText		= string.Empty;

		//==========================================
		//	関数
		//==========================================

		/// <summary>
		/// 
		/// </summary>
		public DisplayDialogStep( string name, string message, string ok, string cancel )
		{
			m_dialogName		= name;
			m_displayMessage    = message;
			m_okText            = ok;
			m_cancelText        = cancel;
		}
		public DisplayDialogStep( string name, string message ) : this( name, message, "OK", "Cancel" ) { }

		protected override BuildResult DoExecute()
		{
			if( PringlesUtils.IsBatchMode )
			{
				return Success( $"[Batch Mode] Skip Dialog : {m_dialogName}" );
			}

			var isSelectOK = EditorUtility.DisplayDialog( m_dialogName, m_displayMessage, m_okText, m_cancelText );
			if( isSelectOK )
			{
				return Success( $"Select : {m_okText}");
			}
			return Error( $"Select : {m_cancelText}");
		}
	}
}