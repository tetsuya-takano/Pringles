using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Pringles
{
	public interface IDefineSymbols : IBuildParameter
	{
		string[] DefineSymbols { get; }
	}
	public class DefineSymbolsParameter : IDefineSymbols
	{
		public DefineSymbolsParameter( string symbols)
		{
			DefineSymbols = symbols.Split(';');
		}
		public DefineSymbolsParameter(string[] symbols)
		{
			DefineSymbols = symbols;
		}

		public string[] DefineSymbols { get; }
	}
}