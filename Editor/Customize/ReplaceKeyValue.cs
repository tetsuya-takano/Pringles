using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pringles
{
	[Serializable]
	public class ReplaceKeyValue
	{
		public string Key = string.Empty;
		[NonSerialized]
		public string Value = string.Empty;

		public override string ToString()
		{
			return $"{Key}:{Value}";
		}
	}
	public static class ReplaceKeyValueExtensions
	{
		public static string Replace(this string self, ReplaceKeyValue replace)
		{
			return self.Replace(replace.Key, replace.Value);
		}
		public static string ReplaceForFlag(this string self, ReplaceKeyValue replace)
		{
			return self.Replace(replace.Key, replace.ToFlag());
		}
		public static string ReplaceForArray(this string self, ReplaceKeyValue replace, char separator = ';' )
		{
			// %xxx% => [ "aaa",\n"bbb",\n ]
			// 分割
			var split = replace.Value.Split(separator);
			var sb = new StringBuilder();
			//
			sb.AppendLine("[");
			for( int i = 0; i< split.Length; i++ )
			{
				var v = split[i];
				sb.Append($"\"{v}\"");
				if (i < split.Length - 1)
				{
					sb.Append(",");
				}
				sb.AppendLine();
			}
			sb.AppendLine("]");
			return self.Replace(replace.Key, sb.ToString());
		}
		private static string ToFlag(this ReplaceKeyValue replace)
		{
			bool.TryParse(replace.Value, out var flag);
			return flag.ToString().ToLower();
		}
	}
}
