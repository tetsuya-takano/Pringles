using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Pringles.Parser
{
    public class StreamingAssetsFileListStepParser : BuildStepParser<StreamingAssetsFileListStep>
    {
        public override string Cmd => "streamingassets-table";

        protected override StreamingAssetsFileListStep DoParse()
        {
            var path = GetArg(ArgsKey.OutputPath);
            var fileName = GetArg(ArgsKey.FileName);

            return new StreamingAssetsFileListStep( path, fileName );
        }
    }
}