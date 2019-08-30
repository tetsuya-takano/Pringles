using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// テンプレートファイルを持ってくる
	/// </summary>
	public abstract class TemplateLocater : ScriptableObject
	{
		public abstract IReadOnlyList<Uri> GetTemplates();

		public abstract Uri GetOriginal(Uri template);

		public abstract Uri GetTemplateRootUri();

		public abstract Uri GetOriginRootUri();
	}
}