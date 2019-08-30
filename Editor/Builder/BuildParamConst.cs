using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// ビルド系の定数
	/// </summary>
	public static class BuildParamConst
	{
	}
	public class ArgsKey
	{
		public const string BundleId = "-bundleId";
		public const string Platform = "-platform";
		public const string ProductName = "-name";

		public const string DevelopFlag = "-develop";
		public const string AllowDebugerFlag = "-debug";
		public const string LZ4CompressFlag = "-lz4";
		public const string LZMACompressFlag = "-lzma";
		public const string Symbols = "-symbols";

		public const string FileName = "-fileName";
		public const string OutputPath = "-output";
	}
}