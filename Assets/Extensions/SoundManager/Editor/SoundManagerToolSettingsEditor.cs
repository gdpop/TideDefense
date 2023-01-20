using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace VirtuoseReality.Extension.AudioManager
{


	[CanEditMultipleObjects]
	[CustomEditor(typeof(SoundManagerToolSettings))]
	public class SoundManagerToolSettingsEditor : Editor
	{

		private SoundManagerToolSettings m_target = null;

		private void OnEnable()
		{
			m_target = target as SoundManagerToolSettings;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if(GUILayout.Button("Generate SoundDataID static class"))
			{
				GenerateSoundDataID();
			}

		}

		private void GenerateSoundDataID()
		{
			string data = " { \r";

			foreach (SoundDataLibrary library in m_target.SoundDataLibraries)
			{
				if (!library.GenerateStatic)
					continue;

				data += string.Format(TEMPLATE_SOUND_DESCRIPTION, library.name);

				foreach (SoundData soundData in library.SoundDatas)
					data += string.Format("public static readonly string {0} = \"{1}\"; \r", UpperCamelCaseToConstant(soundData.ID), soundData.ID);

			}

			data += " } \r ";

			m_target.generatedSoundDataID = string.Format(TEMPLATE_SOUND_DATA_ID, data);
		}

		public string UpperCamelCaseToConstant(string data)
		{
			string result = "";
			MatchCollection collection = Regex.Matches(data, @"[A-Z][a-z]*\d*");
			//MatchCollection collection = Regex.Matches(data, @"[A-Z][a-z]+");
			for (int i = 0; i < collection.Count - 1; i++)
				result += (collection[i].Value + "_").ToUpper();

			result += collection[collection.Count -1].Value.ToUpper();
			return result;
		}

		private const string TEMPLATE_SOUND_DESCRIPTION =
		@"
			/*
				{0}
			*/
		";

		private const string TEMPLATE_SOUND_DATA_ID =
		@"
			public static class SoundDataIDStatic
			{0}
			
		";

	}
}