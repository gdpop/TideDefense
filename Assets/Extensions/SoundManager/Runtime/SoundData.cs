using System;
using UnityEngine;
using UnityEngine.Audio;

namespace VirtuoseReality.Extension.AudioManager
{
	/// <summary>
	/// Custom class for an AudioClip
	/// </summary>
	[Serializable]
	public class SoundData
	{
		[Header("Base Properties")]
		[SerializeField] private string m_ID = "";
		[SerializeField] private AudioClip m_clip = null;

		[SerializeField] private AudioMixerGroup m_mixer = null;

		private SoundType m_type = SoundType.None;

		public string ID { get { return m_ID; } }
		public AudioClip Clip { get { return m_clip; } }
		public SoundType Type { get { return m_type; } }
		public AudioMixerGroup Mixer { get { return m_mixer; } }

		public void SetType(SoundType type)
		{
			m_type = type;
		}

		public SoundData() { }

		public SoundData(string ID, AudioClip clip, AudioMixerGroup mixer)
		{
			m_ID = ID;
			m_clip = clip;
			m_mixer = mixer;
		}

	}

}