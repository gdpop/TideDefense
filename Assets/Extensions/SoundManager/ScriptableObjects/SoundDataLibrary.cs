using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace VirtuoseReality.Extension.AudioManager
{

	/// <summary>
	/// Collection of a specific type of SoundDatas
	/// </summary>
	[CreateAssetMenu(fileName = "SoundDataLibrary", menuName = "Extensions/AudioManager/SoundDataLibrary", order = 1)]
	public class SoundDataLibrary : ScriptableObject
	{

		#region Fields

		[Tooltip("Short description of the library")]
		[SerializeField, TextArea(0, 10)] private string m_description = "";

		[Tooltip("Type of library")]
		[SerializeField] private SoundType m_type = SoundType.None;

		[Tooltip("Library data of SoundDatas")]
		[Space(40)]
		[SerializeField] private List<SoundData> m_soundDatas = null;

		[SerializeField] private bool m_generateStatic = true;

		public string Description { get { return (m_description); } }
		public List<SoundData> SoundDatas { get { return m_soundDatas; } }
		public SoundType Type { get { return m_type; } }
		public bool GenerateStatic { get { return m_generateStatic; } }


		[Header("Fast fill")]
		[SerializeField] private List<AudioClip> m_audioClips = new List<AudioClip>();
		[SerializeField] private AudioMixerGroup m_mixerGroup = null;

		public List<AudioClip> audioClips { get { return m_audioClips; } }
		public AudioMixerGroup mixerGroup { get { return m_mixerGroup; } }

		#endregion

		#region Methods

		/// <summary>
		/// Initialize the library
		/// </summary>
		public void Init()
		{
			// Set the type if all SoundType
			for(int i = 0; i < m_soundDatas.Count; i++)
				m_soundDatas[i].SetType(m_type);

		}

		#endregion

	}

}