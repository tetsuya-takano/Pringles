using UnityEngine;
using UnityEditor;

namespace Pringles
{
	/// <summary>
	/// ただのJsonテキストで扱うやつ
	/// </summary>
	public sealed class RawJsonConverter : ConfigConverter
	{
		public override T Deserialize<T>(byte[] datas)
		{
			var str = System.Text.Encoding.UTF8.GetString(datas);
			return JsonUtility.FromJson<T>(str);
		}

		public override byte[] Serialize(string contents)
		{
			return System.Text.Encoding.UTF8.GetBytes(contents);
		}
	}
}