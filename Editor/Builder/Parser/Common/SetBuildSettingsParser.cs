using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Pringles.Parser
{
    /// <summary>
    /// ビルド設定を埋めるやつ
    /// </summary>
    public abstract class SetBuildSettingsParser : BuildStepParser<EmptyStep>
    {
        //=================================
        // 関数
        //=================================

        /// <summary>
        /// 
        /// </summary>
        protected sealed override EmptyStep DoParse()
        {
            DoSetContext();
            return EmptyStep.Default;
        }

        protected abstract void DoSetContext();
    }

    /// <summary>
    /// ビルドオプション用
    /// </summary>
    public sealed class SetBuildOptionParser : SetBuildSettingsParser
    {
        public override string Cmd => "set-build-option";

        protected override void DoSetContext()
        {
            var p = new BuildOptionParameter(
                isDevBuild : HasArg(ArgsKey.DevelopFlag), 
                allowDebug : HasArg(ArgsKey.AllowDebugerFlag), 
                isCompress : HasArg(ArgsKey.LZ4CompressFlag)
            );
            Context.Set( p );
        }
    }
    
    /// <summary>
    /// 出力先用
    /// </summary>
    public sealed class SetLocationPathParser : SetBuildSettingsParser
    {
        public override string Cmd => "set-location-path";

        protected override void DoSetContext()
        {
            var p = new LocationPathParameter(GetArg(ArgsKey.OutputPath));
            Context.Set(p);
        }
    }
    /// <summary>
    /// Rom名用
    /// </summary>
    public sealed class SetProductNameParser : SetBuildSettingsParser
    {
        public override string Cmd => "set-product-name";

        protected override void DoSetContext()
        {
            var p = new ProductNameParameter(GetArg(ArgsKey.ProductName));
            Context.Set(p);
        }
    }
    /// <summary>
    /// BundleId用
    /// </summary>
    public sealed class SetBundleIdParser : SetBuildSettingsParser
    {
        public override string Cmd => "set-bundleId";

        protected override void DoSetContext()
        {
            var p = new BundleIdParameter(GetArg(ArgsKey.BundleId));
            Context.Set(p);
        }
    }
    /// <summary>
    /// プラットフォーム用
    /// </summary>
    public sealed class SetBuildTargetParser : SetBuildSettingsParser
    {
        public override string Cmd => "set-build-target";

        protected override void DoSetContext()
        {
            var p = new BuildTargetParameter(GetArg(ArgsKey.Platform));
            Context.Set(p);
        }
    }

    /// <summary>
    /// 対象シーン用
    /// </summary>
    public sealed class SetBuildScenesParser : SetBuildSettingsParser
    {
        public override string Cmd => "set-build-scene";

        protected override void DoSetContext()
        {
            var p = new BuildScenesParameter();
            Context.Set(p);
        }
    }
}