using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Pringles.Parser
{
	public class SetAppSettingsParser : SetBuildSettingsParser
	{
		public override string Cmd => "set-app-settings";

		protected override void DoSetContext()
		{
			var p = new BuildTargetParameter( GetArg(ArgsKey.Platform));
			var fileName = PringlesUtils.ToReplaceDefinedPath( GetArg(ArgsKey.FileName) );
			var settings = Read(p.Target, fileName);
			Context.Set( settings );
		}

		private IBuildSettings Read( BuildTarget platform, string path )
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException(path);
			}
			var json = File.ReadAllText( path );
			var settings = JsonUtility.FromJson<BuildSettingsJson>( json );
			if (settings.Target == platform)
			{
				return settings;
			}
			else
			{
				throw new Exception($"Platform MissMatch :: args = {platform}, settings={settings.Target}");
			}

			throw new NotSupportedException(platform.ToString());
		}
	}
}