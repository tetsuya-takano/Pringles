using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
	public abstract class ConfigObject<TFormat> : ConfigObject
	{
	}
	/// <summary>
	/// 何かの設定ファイルを生成して実行時に扱うための何か
	/// </summary>
	public abstract class ConfigObject : ScriptableObject
	{
		//===================================
		// SerializeField
		//===================================
		[SerializeField] private ConfigConverter m_converter = default;

		[SerializeField] private byte[] m_datas = default; // 対象のファイル

		//===================================
		// Property
		//===================================
		private object CachedObject { get; set; }

		//===================================
		// Method
		//===================================

		/// <summary>
		/// 取得
		/// </summary>
		public TFormat Get<TFormat>( bool reload = false )
		{
			if( !reload && CachedObject != null)
			{
				return (TFormat)CachedObject;
			}
			return (TFormat)(CachedObject = Deserialize<TFormat>());
		}
		public TFormat Deserialize<TFormat>()
		{
			return m_converter.Deserialize<TFormat>(m_datas);
		}

		public void Serialize(string contents)
		{
			m_datas = m_converter.Serialize(contents);
		}
	}
}