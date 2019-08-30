using UnityEngine;
using UnityEditor;

namespace Pringles
{
	/// <summary>
	/// Configファイルの変換方法を記述する
	/// </summary>
	public abstract class ConfigConverter : ScriptableObject
	{
		public abstract T Deserialize<T>( byte[] datas );
		public abstract byte[] Serialize( string contents );
	}
}