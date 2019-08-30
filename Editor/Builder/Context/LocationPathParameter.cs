using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
    public interface ILocationPath : IBuildParameter
    {
        string LocationPath { get; }
    }
    public class LocationPathParameter : ILocationPath
    {
        public string LocationPath { get; }

        public LocationPathParameter(string path)
        {
            LocationPath = path;
        }
    }
}