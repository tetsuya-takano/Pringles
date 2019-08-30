using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Pringles.Parser
{
    /// <summary>
    /// Romビルド処理ステップ
    /// </summary>
    public sealed class ApplicationBuildStepParser : BuildStepParser<ApplicationBuildStep>
    {
        public override string Cmd => "app-build";

        protected override ApplicationBuildStep DoParse()
        {
            var name = Context?.Get<IProductName>()?.ProductName ?? GetArg(ArgsKey.ProductName);
            var path = PringlesUtils.ToReplaceDefinedPath(Context?.Get<ILocationPath>()?.LocationPath ?? GetArg("-outputPath"));
            var bundleId = Context?.Get<IBundleId>()?.BundleId ??  GetArg(ArgsKey.BundleId);
            var buildTarget = Context?.Get<IBuildTarget>() ?? new BuildTargetParameter(GetArg(ArgsKey.Platform) );
            var levels = Context?.Get<IBuildScenes>()?.Levels;
            var options = Context?.Get<IBuildOption>()?.Option
                ?? new BuildOptionParameter(
                    isDevBuild : HasArg(ArgsKey.DevelopFlag), 
                    allowDebug : HasArg(ArgsKey.AllowDebugerFlag), 
                    isCompress : HasArg(ArgsKey.LZ4CompressFlag)
                ).Option;

            return new ApplicationBuildStep
            (
                productName : name,
                outputPath : path,
                levels  : levels,
                bundleId: bundleId,
                options : options,
                group   : buildTarget.Group,
                target  : buildTarget.Target
            );
        }
    }
}
