using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace Pringles.Parser
{
    public interface IBuildStepParser
    {
        string Cmd { get;  }
        IBuildStep Parse(IBuildContext context, string[] args);
    }

    /// <summary>
    /// 文字列からステップを生成するやつ
    /// </summary>
    public abstract class BuildStepParser<T> : IBuildStepParser
        where T : IBuildStep
    {
        //=============================================
        // プロパティ
        //=============================================
        public abstract string Cmd { get; }
        private string[] Args { get; set; }
        protected IBuildContext Context { get; private set; }

        //=============================================
        // 関数
        //=============================================

        /// <summary>
        /// 
        /// </summary>
        public IBuildStep Parse(IBuildContext context, string[] args)
        {
            Context = context;
			Args = ConvertEnv(args);
            return DoParse();
        }
		private string[] ConvertEnv( string[] args )
		{
			var tmp = args;

			for (var i = 0; i < tmp.Length; i++)
			{
				tmp[i] = ParserUtils.ReplaceEnv(tmp[i]);
			}
			return tmp;
		}

        protected abstract T DoParse();

        protected string GetArg(string name)
        {
            int index = Array.IndexOf( Args, name );
            if (index < 0)
            {
                UnityEngine.Debug.LogWarningFormat("NotFound Args :{0}", name);
                return string.Empty;
            }
            // キーの次が値
            var arg = Args.ElementAtOrDefault(index + 1 );
            if (string.IsNullOrEmpty(arg))
            {
                UnityEngine.Debug.LogWarningFormat("Arg is Null :{0}", arg);
                return string.Empty;
            }
            // オプションだけのこともある
            if (arg.StartsWith("-"))
            {
                UnityEngine.Debug.LogWarningFormat("Can't Use Arg : {0}", arg);
                return string.Empty;
            }
            return arg;
        }

        protected bool HasArg( string name )
        {
            return Args.Contains(name);
        }
    }

    public sealed class EmptyParser : IBuildStepParser
    {
        public static EmptyParser Default { get; } = new EmptyParser();

        public string Cmd => "Empty";

        public IBuildStep Parse(IBuildContext context, string[] args)
        {
            return EmptyStep.Default;
        }
    }
}
