using System;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// 結果
	/// </summary>
	public enum ResultType
	{
		Success	,
		Warning	,
		Error	,
	}
	/// <summary>
	/// 
	/// </summary>
	public struct BuildResult
	{
		public string		Tag		{ get; set; }
		public string		Message { get; set; }
		public ResultType	Type	{ get; set; }

		public override string ToString()
		{
			return string.Format( "[{0}] {1} \n{2}", Tag, Type, Message );
		}
	}
	public interface IBuildStep : IDisposable
	{
		BuildResult Execute();
	}
	/// <summary>
	/// ビルドの1処理を作成するクラス
	/// </summary>
	public abstract class BuildStep : IBuildStep
	{
		//========================
		//	関数
		//========================
		protected abstract string Tag { get; }

		//========================
		//	関数
		//========================

		/// <summary>
		/// 破棄
		/// </summary>
		public void Dispose()
		{
			DoDispose();
		}
		protected virtual void DoDispose() { }

		public BuildResult Execute()
		{
			return DoExecute();
		}

		protected abstract BuildResult DoExecute();

		protected BuildResult Success( string message )
		{
			return new BuildResult
			{
				Message = message,
				Tag		= Tag,
				Type	= ResultType.Success,
			};
		}
		protected BuildResult Warning( string message )
		{
			return new BuildResult
			{
				Message = message,
				Tag     = Tag,
				Type    = ResultType.Warning,
			};
		}
		protected BuildResult Error( string message )
		{
			return new BuildResult
			{
				Message = message,
				Tag     = Tag,
				Type    = ResultType.Error,
			};
		}

		protected void Log( string msg )
		{
			Debug.Log(msg);
		}

		public override string ToString()
		{
			return string.Format( "[{0}]", Tag );
		}
	}
}