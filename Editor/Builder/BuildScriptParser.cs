using Pringles.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pringles
{
	/// <summary>
	/// ビルドスクリプトをビルドステップに変換する
	/// </summary>
	public sealed class BuildScriptParser : IEnumerable
	{
		//========================================
		// 変数
		//========================================
		private Dictionary<string, IBuildStepParser> m_container = new Dictionary<string, IBuildStepParser>();
		//========================================
		// 関数
		//========================================
		public BuildScriptParser() { }
		public BuildScriptParser(params IBuildStepParser[] list)
		{
			foreach (var p in list)
			{
				Add(p);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)m_container).GetEnumerator();
		}

		/// <summary>
		/// 作成する
		/// </summary>
		public IBuildStep Parse(IBuildContext context, string line)
		{
			if (string.IsNullOrEmpty(line))
			{
				// 空行
				return EmptyParser.Default.Parse(context, null);
			}
			if (line.StartsWith("#"))
			{
				//コメント行
				return EmptyParser.Default.Parse(context, null);
			}
			// 1ラインの1個めをコマンド名
			var commands = line.Split(' ');
			var cmd = commands.First();
			if (m_container.TryGetValue(cmd, out IBuildStepParser parser))
			{
				// 2個め以降を引数にしてパーサを変換する
				var args = commands.Skip(1).ToArray();
				return parser.Parse(context, args);
			}
			return EmptyParser.Default.Parse(context, null);
		}

		/// <summary>
		/// パーサの登録
		/// </summary>
		public void Add(IBuildStepParser parser)
		{
			if (m_container.ContainsKey(parser.Cmd))
			{
				m_container[parser.Cmd] = parser;
				return;
			}
			m_container.Add(parser.Cmd, parser);
		}
	}
}
