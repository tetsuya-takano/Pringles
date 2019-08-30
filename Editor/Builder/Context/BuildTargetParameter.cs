using Pringles.Parser;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pringles
{
    public interface IBuildTarget : IBuildParameter
    {
        BuildTargetGroup Group { get; }
        BuildTarget Target { get; }
    }
    public sealed class BuildTargetParameter : IBuildTarget
    {
        public BuildTargetGroup Group { get; }
        public BuildTarget Target { get; }

        public BuildTargetParameter( BuildTargetGroup group, BuildTarget target )
        {
            Group = group;
            Target = target;
        }
        public BuildTargetParameter(string platform) : this(
            group : ParserUtils.ToGroup(platform), 
            target: ParserUtils.ToTarget(platform)
        )
        {
        }
    }
}