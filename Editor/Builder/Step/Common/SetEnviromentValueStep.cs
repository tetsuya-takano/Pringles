using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// 環境変数からどこかに値をセットする処理
	/// </summary>
	public abstract class SetEnviromentValueStep : BuildStep
	{
		//==============================
		//	変数
		//==============================
		private readonly string m_key;

		//==============================
		//	関数
		//==============================

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SetEnviromentValueStep(string key)
		{
			m_key = key;
		}

		protected string Get()
		{
			return System.Environment.GetEnvironmentVariable( m_key );
		}

		protected void Set(string value)
		{
			System.Environment.SetEnvironmentVariable(m_key, value);
		}
	}
}