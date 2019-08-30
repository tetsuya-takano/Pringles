using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pringles.Parser
{
    /// <summary>
    /// ファイル削除コマンドを作るやつ
    /// </summary>
    public sealed class DeleteStepParser : BuildStepParser<DeleteStep>
    {
        public override string Cmd => "rm";

        protected override DeleteStep DoParse()
        {
            var dirPath = PringlesUtils.ToReplaceDefinedPath( GetArg("-path") );
            var pattern = GetArg("-pattern");
            if (string.IsNullOrEmpty(pattern))
            {
                return new DeleteDirectoryStep( dirPath );
            }
            return new DeleteFileStep(dirPath, pattern);
        }
    }
}
