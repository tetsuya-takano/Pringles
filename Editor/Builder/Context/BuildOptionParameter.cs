using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pringles
{
    public interface IBuildOption : IBuildParameter
    {
        BuildOptions Option { get; }
    }
    public class BuildOptionParameter : IBuildOption
    {
        public BuildOptions Option { get; }

        public BuildOptionParameter( bool isDevBuild, bool allowDebug, bool isCompress )
        {
            Option = BuildOptions.None;

            if (isDevBuild)
            {
                Option |= BuildOptions.Development;
            }
            if (allowDebug)
            {
                Option |= BuildOptions.AllowDebugging;
            }

            if (isCompress)
            {
                Option |= BuildOptions.CompressWithLz4;
            }
        }
    }
}