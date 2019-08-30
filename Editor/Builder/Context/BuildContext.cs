using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
    public interface IBuildParameter { }
    public interface IBuildContext
    {
        void Set<T>(T parameter) where T : IBuildParameter;
        T Get<T>() where T : IBuildParameter;
    }
    /// <summary>
    /// 
    /// </summary>
    public class BuildContext : IBuildContext
    {
        //=====================================
        // 変数
        //=====================================
        private List<IBuildParameter> m_itemList = new List<IBuildParameter>();

        //=====================================
        // 関数
        //=====================================

        /// <summary>
        /// 追加
        /// </summary>
        public void Set<T>(T parameter) where T : IBuildParameter
        {
            if (m_itemList.Contains( parameter ))
            {
                return;
            }
            m_itemList.Add( parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        public T Get<T>() where T : IBuildParameter
        {
            foreach (var p in m_itemList)
            {
                if (p is T)
                {
                    return (T)p;
                }
            }
            return default;
        }
    }
}