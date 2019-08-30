using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles.Parser
{
	/// <summary>
	/// シンボル設定
	/// </summary>
	public class SetSymbolsStepParser : BuildStepParser<SetDefineSymbolsStep>
	{
		public override string Cmd => "set-symbols";

		protected override SetDefineSymbolsStep DoParse()
		{
			var g = (Context?.Get<IBuildTarget>() ?? new BuildTargetParameter(GetArg(ArgsKey.Platform))).Group;
			var path = PringlesUtils.ToReplaceDefinedPath(GetArg("-path"));
			if (!string.IsNullOrEmpty(path))
			{
				return SetDefineSymbolsStep.Read(g, path);
			}
			var symbols = GetArg(ArgsKey.Symbols).Split(';');
			return new SetDefineSymbolsStep(g, symbols);

		}
	}
	/// <summary>
	/// シンボル追加
	/// </summary>
	public class AddSymbolsStepParser : BuildStepParser<AppendDefineSymbolsStep>
	{
		public override string Cmd => "add-symbols";

		protected override AppendDefineSymbolsStep DoParse()
		{
			var g = (Context?.Get<IBuildTarget>() ?? new BuildTargetParameter(GetArg(ArgsKey.Platform))).Group;
			var symbols = GetArg(ArgsKey.Symbols).Split(';');
			return new AppendDefineSymbolsStep(g, symbols);

		}
	}
}