using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pringles.Parser
{
    public static class DefaultScriptParser
    {
        /// <summary>
        /// デフォルト機能
        /// </summary>
        public static BuildScriptParser Default { get; } = new BuildScriptParser
        {
            new CopyFileStepParser(),
            new DeleteStepParser(),
            new ApplicationBuildStepParser(),
            new SetSymbolsStepParser(),
            new AddSymbolsStepParser(),
            new SwitchPlatformStepParser(),
            new DisplayDialogStepParser(),

            new ResourcesFileListStepParser(),
            new StreamingAssetsFileListStepParser(),

            new SetLocationPathParser(),
            new SetBuildOptionParser(),
            new SetBuildScenesParser(),
            new SetBuildTargetParser(),
            new SetBundleIdParser(),
            new SetProductNameParser(),
			new SetAppSettingsParser()
        };
    }
}
