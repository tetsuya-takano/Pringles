using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// 
	/// </summary>
	public interface IMatchRule
	{
		bool IsMatch( string value );
	}

	/// <summary>
	/// 素通し
	/// </summary>
	public sealed class ThroughRule : IMatchRule
	{
		public bool IsMatch(string value) => true;

		public override string ToString()
		{
			return "Rule::"+"All for True";
		}
	}

	public sealed class ConditionMatchRule : IMatchRule
	{
		private Func<string, bool> m_condition = null;
		public ConditionMatchRule(Func<string, bool> condition)
		{
			m_condition = condition;
		}
		public bool IsMatch(string value)
		{
			return m_condition?.Invoke( value ) ?? false ;
		}
		public override string ToString()
		{
			return "Rule:: From Condition";
		}
	}

	/// <summary>
	/// 正規表現マッチング
	/// </summary>
	public sealed class RegexMatchRule : IMatchRule
	{
		//============================
		// 変数
		//============================
		private Regex[] m_regices = new Regex[0];
		//============================
		// 関数
		//============================

		public RegexMatchRule(string pattern) : this(new string[] { pattern })
		{

		}
		public RegexMatchRule( string[] patterns)
		{
			m_regices = patterns.Select(c => new Regex(c)).ToArray();
		}

		public bool IsMatch( string value )
		{
			foreach( var r in m_regices)
			{
				if( r.IsMatch( value))
				{
					return true;
				}
			}
			return false;
		}

		public override string ToString()
		{
			return "Rule :: Regex::" + string.Join(",", m_regices.SelectMany(c => c.GetGroupNames()));
		}
	}

	/// <summary>
	/// 反転
	/// </summary>
	public sealed class RuleReverce : IMatchRule
	{
		private IMatchRule m_rule = null;

		public RuleReverce( IMatchRule rule )
		{
			m_rule = rule;
		}

		public bool IsMatch(string value)
		{
			return !m_rule.IsMatch(value);
		}
		public override string ToString()
		{
			return $"Reverce:{m_rule.ToString()}";
		}
	}


	public static class MatchRuleExtensions
	{
		/// <summary>
		/// 反転させる
		/// </summary>
		public static IMatchRule Reverce( this IMatchRule self )
		{
			return new RuleReverce(self);
		}
	}
}