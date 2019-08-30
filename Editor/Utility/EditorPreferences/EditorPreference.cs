using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Pringles
{
	/// <summary>
	/// Preferenceの値を操作するためのモノ
	/// </summary>
	public static partial class EditorPreference
	{
		//=========================================
		//	class
		//=========================================

		/// <summary>
		/// EditorPrefsの1要素を管理する
		/// </summary>
		public abstract class PrefsData<T>
		{
			public string Key { get; }
			public abstract T Value { get; }
			public abstract void Set(T value );
			public PrefsData( string key )
			{
				Key = key;
			}
            public static implicit operator T(PrefsData<T> self) => self.Value;
        }

		public sealed class IntPrefsData : PrefsData<int>
		{
			public IntPrefsData(string key) : base(key) { }
			public override int Value => EditorPrefs.GetInt(Key);
			public override void Set(int value) => EditorPrefs.SetInt(Key, value);

            public static implicit operator int(IntPrefsData self) => self.Value;

        }
        public sealed class BoolPrefsData : PrefsData<bool>
		{
			public BoolPrefsData(string key) : base(key) { }
			public override bool Value => EditorPrefs.GetBool(Key);
			public override void Set(bool value) => EditorPrefs.SetBool(Key, value);

            public static implicit operator bool(BoolPrefsData self) => self.Value;
        }
		public sealed class FloatPrefsData : PrefsData<float>
		{
			public FloatPrefsData(string key) : base(key) { }
			public override float Value => EditorPrefs.GetInt(Key);
			public override void Set(float value) => EditorPrefs.SetFloat(Key, value);
            public static implicit operator float(FloatPrefsData self) => self.Value;
        }
		public sealed class StringPrefsData : PrefsData<string>
		{
			public StringPrefsData(string key) : base(key) { }
			public override string Value => EditorPrefs.GetString(Key);
			public override void Set(string value) => EditorPrefs.SetString(Key, value);
            public static implicit operator string(StringPrefsData self) => self.Value;
        }
	}
}