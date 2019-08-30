using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pringles.Parser
{
    public static class ParserUtils
    {
        //===========================================
        // static
        //===========================================
        private static Dictionary<string, BuildTargetGroup> m_groupTable = new Dictionary<string, BuildTargetGroup>(StringComparer.OrdinalIgnoreCase);
        private static Dictionary<string, BuildTarget> m_targetTable = new Dictionary<string, BuildTarget>(StringComparer.OrdinalIgnoreCase);

		private static Regex m_EnvRegex = new Regex(@"\$\{(.*?)\}"); // 環境変数一致用
        //===========================================
        // method
        //===========================================
        static ParserUtils()
        {
            // String to BuildTargetGroup
            m_groupTable.Add("android", BuildTargetGroup.Android);
            m_groupTable.Add("ios", BuildTargetGroup.iOS);
            m_groupTable.Add("win", BuildTargetGroup.Standalone);
            m_groupTable.Add("win64", BuildTargetGroup.Standalone);
            m_groupTable.Add("macos", BuildTargetGroup.Standalone);

            // String to BuildTarget
            m_targetTable.Add("android", BuildTarget.Android);
            m_targetTable.Add("ios", BuildTarget.iOS);
            m_targetTable.Add("win", BuildTarget.StandaloneWindows);
            m_targetTable.Add("win64", BuildTarget.StandaloneWindows64);
            m_targetTable.Add("mac", BuildTarget.StandaloneOSX);
        }


        /// <summary>
        /// 
        /// </summary>
        public static BuildTargetGroup ToGroup( string platform )
        {
            if (m_groupTable.TryGetValue(platform, out var group ))
            {
                return group;
            }

            throw new NotSupportedException($" BuildTargetGroup :: {platform} ? {string.Join("|", m_groupTable.Keys)}");
        }
        public static BuildTarget ToTarget(string platform)
        {
            if (m_targetTable.TryGetValue(platform, out var t))
            {
                return t;
            }
            throw new NotSupportedException($" BuildTarget :: {platform} ? {string.Join("|", m_targetTable.Keys)}");
        }

		/// <summary>
		/// 環境変数から差し替え
		/// </summary>
		public static string ReplaceEnv( string source )
		{
			var matches = m_EnvRegex.Matches( source );
			if( matches.Count <= 0)
			{
				return source;
			}
			var tmp = source;
			for( int i = 0; i < matches.Count; i++)
			{
				var group = matches[i].Groups;
				var replaceKey = group[0].Value;
				var envKey = group[1].Value;
				var envValue = Environment.GetEnvironmentVariable( envKey );
				if (string.IsNullOrEmpty(envValue))
				{
					continue;
				}
				tmp = tmp.Replace( replaceKey, envValue );
			}
			return tmp;
		}
    }
}