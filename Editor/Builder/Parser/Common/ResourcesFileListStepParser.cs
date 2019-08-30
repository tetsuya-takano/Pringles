using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles.Parser
{
    /// <summary>
    /// Resourcesのファイルリストを作成する
    /// </summary>
    public sealed class ResourcesFileListStepParser : BuildStepParser<ResourcesFileListBuildStep>
    {
        public override string Cmd => "resources-table";

        protected override ResourcesFileListBuildStep DoParse()
        {
            var path = GetArg(ArgsKey.OutputPath);
            var fileName = GetArg(ArgsKey.FileName);
            return new ResourcesFileListBuildStep(path, fileName);
        }
    }
}