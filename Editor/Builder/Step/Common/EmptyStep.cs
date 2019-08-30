using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
    /// <summary>
    /// 何もしないをする
    /// </summary>
    public sealed class EmptyStep : IBuildStep
    {
        public static EmptyStep Default { get; } = new EmptyStep();
        public void Dispose() { }

        public BuildResult Execute()
        {
            return new BuildResult
            {
                Message = string.Empty,
                Tag = "Empty",
                Type = ResultType.Success
            };
        }
    }
}