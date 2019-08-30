using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Pringles
{
	public abstract class DefineSymbolsStepBase: BuildStep
	{
		//==================================
		//	const
		//==================================

		//==================================
		//	プロパティ
		//==================================

		//===============================
		//	変数
		//===============================
		private readonly BuildTargetGroup   m_group;
		protected readonly string[]           m_symbols;

		//=================================
		//	関数
		//=================================

		public DefineSymbolsStepBase( BuildTargetGroup group, string[] symbols )
		{
			m_group     = group;
			m_symbols   = symbols;
		}

		protected override BuildResult DoExecute()
		{
			var result = string.Join("\n",Get());
			return Success( result );
		}

		protected void Set( string[] symbols )
		{
			var symbolsStr = symbols?.Length == 0 ? string.Empty : string.Join( ";", symbols );
			PlayerSettings.SetScriptingDefineSymbolsForGroup( m_group, symbolsStr );
		}

		protected string[] Get()
		{
			return PlayerSettings.GetScriptingDefineSymbolsForGroup( m_group ).Split( ';' );
		}
	}

	/// <summary>
	/// シンボルの受付
	/// </summary>
	public sealed class SetDefineSymbolsStep : DefineSymbolsStepBase
	{
		//============================================
		//	プロパティ
		//============================================
		protected override string Tag { get { return "Set Define Symbols"; } }

		//============================================
		//	関数
		//============================================
		/// <summary>
		/// ファイルから取得
		/// </summary>
		public static SetDefineSymbolsStep Read( BuildTargetGroup group, string filePath )
		{
			var symbols = File.ReadAllLines( filePath );

			return new SetDefineSymbolsStep( group, symbols );
		}
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SetDefineSymbolsStep( BuildTargetGroup group, string[] symbols ) : base( group, symbols )
		{
		}
		public SetDefineSymbolsStep(BuildTargetGroup group, IDefineSymbols defineSymbols) : base(group, defineSymbols.DefineSymbols) { }
		protected override BuildResult DoExecute()
		{
			//	上書き
			Set( m_symbols );

			return base.DoExecute();
		}
	}

	/// <summary>
	/// シンボルの追加
	/// </summary>
	public sealed class AppendDefineSymbolsStep : DefineSymbolsStepBase
	{
		//==================================
		//	プロパティ
		//==================================
		protected override string Tag { get { return "Append Define Symbols"; } }

		//==================================
		//	関数
		//==================================

		public AppendDefineSymbolsStep( BuildTargetGroup group, string[] symbols ) 
			: base( group, symbols )
		{
		}
		/// <summary>
		/// 
		/// </summary>
		protected override BuildResult DoExecute()
		{
			//	追加
			var current = Get();
			var after   = current.Union( m_symbols ).ToArray();
			Set( after );
			return base.DoExecute();
		}
	}
}
