using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// ビルド処理構築クラス
	/// </summary>
	public sealed class BuildPipelineBuilder : IDisposable, IEnumerable
    {
        //=======================================
        //	const
        //=======================================

        //=======================================
        //	変数
        //=======================================
        private List<IBuildStep> m_steps = new List<IBuildStep>();
        private IBuildContext m_context = null;

		//=======================================
		//	関数
		//=======================================
		public BuildPipelineBuilder(IBuildContext context = null )
		{
			m_context = context ?? new BuildContext();
		}
		/// <summary>
		/// ビルドスクリプトファイルから作成
		/// </summary>
		public BuildPipelineBuilder(string filePath, BuildScriptParser parser, IBuildContext context = null) :
            this(new StreamReader(filePath), parser, context )
        {
        }
		public BuildPipelineBuilder(TextReader reader, BuildScriptParser parser, IBuildContext context) : this(context)
		{
            try
            {
                while (reader.Peek() >= 0)
                {
                    var line = reader.ReadLine();
                    var step = parser.Parse(m_context, line);
                    if (step is EmptyStep)
                    {
                        continue;
                    }
                    Append(step);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                reader.Dispose();
            }
        }

		/// <summary>
		/// 実行
		/// </summary>
		public BuildResult Run()
		{
			BuildResult result = new BuildResult();

			using (var dialog = new ProgressScope( "BuildPipeline" ))
			{
				//	ビルド処理を順番に行っていく
				PringlesUtils.Log("---------------------[ Build Start ]---------------------");
				for (var i = 0; i < m_steps.Count; i++)
				{
					var step = m_steps[i];
					var progress = Mathf.InverseLerp(0, m_steps.Count, i);
					dialog.Show(step.ToString(), progress);
					PringlesUtils.Log($"---------------------[ Start : {step.ToString()} ]---------------------");
					result = step.Execute();
					if (result.Type == ResultType.Error)
					{
						break;
					}
					PringlesUtils.Log($"---------------------[ Done : { result.ToString() } ]---------------------");
				}
				PringlesUtils.Log($"---------------------[ Build Finish : {result.Type } ]---------------------");
				return result;
			}
		}

		/// <summary>
		/// 追加
		/// </summary>
		public BuildPipelineBuilder Append( IBuildStep step )
		{
			Add( step );

			return this;
		}

		/// <summary>
		/// 破棄
		/// </summary>
		public void Dispose()
		{
			m_steps.Clear();
		}
		public override string ToString()
		{
			var builder = new StringBuilder();
			foreach( var step in m_steps)
			{
				builder.AppendLine( step.ToString() );
			}
			return builder.ToString();
		}

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)m_steps).GetEnumerator();

        private void Add(IBuildStep step) => m_steps.Add(step);
    }
}