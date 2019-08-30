using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
    public interface IProductName : IBuildParameter
    {
        string ProductName { get; }
    }
    public class ProductNameParameter : IProductName
    {
        public string ProductName { get; }

        public ProductNameParameter(string name)
        {
            ProductName = name;
        }
    }
}
