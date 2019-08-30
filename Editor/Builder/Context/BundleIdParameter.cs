using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
    public interface IBundleId : IBuildParameter
    {
        string BundleId { get; }
    }
    public class BundleIdParameter : IBundleId
    {
        public string BundleId { get; }

        public BundleIdParameter(string bundleId)
        {
            BundleId = bundleId;
        }
    }
}