using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pringles.Parser
{
	/// <summary>
	/// ファイルコピーコマンドを作るやつ
	/// </summary>
	public sealed class CopyFileStepParser : BuildStepParser<CopyFileStep>
	{
		public override string Cmd => "cp";

		protected override CopyFileStep DoParse()
		{
			var sourceDirPath = GetArg("-sourcePath");
			var targetDirPath = GetArg("-targetPath");
			var mode = CopyFileStep.Option.Error;
			if(HasArg("-f"))
			{
				mode = CopyFileStep.Option.None;
			}
			if (HasArg("-i"))
			{
				mode = CopyFileStep.Option.Warning;
			}
			if (HasArg("-n"))
			{
				mode = CopyFileStep.Option.DryRun;
			}

			var pattern = GetArg("-pattern");
			if (HasArg("-ignoreFile"))
			{
				var ignoreFilePath = GetArg("-ignoreFile");
				return CopyFileStep.FromPatternFile(sourceDirPath, targetDirPath, pattern, ignoreFilePath, mode);
			}
			if (HasArg("-ignorePattern"))
			{
				var ignorePattern = GetArg("-ignorePattern");
				var ignoreList = ignorePattern.Split(',');
				return CopyFileStep.FromPatternInput(sourceDirPath, targetDirPath, pattern, ignoreList, mode);
			}
			return CopyFileStep.FromPatternInput(sourceDirPath, targetDirPath, pattern, mode);
		}
	}
}
