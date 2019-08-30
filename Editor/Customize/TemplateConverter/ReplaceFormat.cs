using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// 置き換えルールファイル
	/// </summary>
	public abstract class ReplaceFormat : ScriptableObject
	{
		public abstract string Replace(string content);
	}
}