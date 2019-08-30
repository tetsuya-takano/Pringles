using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pringles
{
	/// <summary>
	/// プラットフォーム切り替え
	/// </summary>
	public sealed class PlatformSwitchStep : BuildStep
	{
		//==================================
		//	プロパティ
		//==================================
		protected override string Tag { get { return "Switch Platform"; } }

		//==================================
		//	変数
		//==================================
		private readonly BuildTargetGroup m_group = BuildTargetGroup.Unknown;
		private readonly BuildTarget      m_target= BuildTarget.NoTarget;

		//==================================
		//	関数
		//==================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public PlatformSwitchStep( BuildTargetGroup group, BuildTarget target )
		{
			m_group = group;
			m_target= target;
		}
		public PlatformSwitchStep(IBuildTarget target) : this(target.Group, target.Target)
		{
		}

		/// <summary>
		/// 実行
		/// </summary>
		protected override BuildResult DoExecute()
		{
			var currentTarget = EditorUserBuildSettings.activeBuildTarget;
			if( currentTarget == m_target )
			{
				return Success
				(
					$"Is Same. No Change Target :{m_target}"
				);
			}
			if( !EditorUserBuildSettings.SwitchActiveBuildTarget( m_group, m_target ) )
			{
				return Error( $"Can't Switch Target : {m_group},{m_target}");
			}
			return Success( $"Change Target : {currentTarget} -> {m_target}");
		}
	}
}