using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Pringles
{
    /// <summary>
    /// OBBの設定をするStep
    /// </summary>
    public sealed class AndroidSplitBinaryStep : BuildStep
    {
        //==============================================
        //  プロパティ
        //==============================================
        protected override string Tag { get { return "Set Split Binary"; } }

        //==============================================
        //  変数
        //==============================================
        private readonly bool m_isSplit = false;

        //==============================================
        //  関数
        //==============================================

        public AndroidSplitBinaryStep(bool isSplit )
        {
            m_isSplit = isSplit;
        }
		public AndroidSplitBinaryStep(ISplitBinary split) : this(split.IsUseObb)
		{
		}
		protected override BuildResult DoExecute()
        {
            PlayerSettings.Android.useAPKExpansionFiles = m_isSplit;

            return Success($"Split Binary : {m_isSplit}");
        }
    }
}