using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Pringles
{
    public interface IBuildScenes : IBuildParameter
    {
        string[] Levels { get; }
    }
    public class BuildScenesParameter : IBuildScenes
    {
        public string[] Levels { get; }


        public BuildScenesParameter( )
        {
            var scenes = EditorBuildSettings.scenes;

            Levels = scenes
                .Where(c => c.enabled)
                .Select(c => c.path)
                .ToArray();
        }
    }
}