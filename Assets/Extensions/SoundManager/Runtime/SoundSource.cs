using DG.Tweening;
using System;
using UnityEngine;

namespace VirtuoseReality.Extension.AudioManager
{

	/// <summary>
	/// Custom behaviour class for Audio Source
	/// </summary>
	[ExecuteInEditMode, RequireComponent(typeof(AudioSource))]
	public class SoundSource : MonoBehaviour
	{

		#region Fields

		[Header("Base Properties")]
		[Tooltip("ID of the SoundSource")]
		[SerializeField] private string m_ID = "SoundSource_";

		[Tooltip("Type of sounds it plays")]
		[SerializeField] private SoundType m_soundType = SoundType.None;

		[Tooltip("Status of the SoundSource")]
		[SerializeField] private ActivityStatus m_status = ActivityStatus.None;

		private AudioSource m_audioSource = null;
		private SoundData m_currentSoundData = null;
		private float m_volumeBeforeMute = 0f;

		public string ID { get { return m_ID; } }
		public AudioSource AudioSource { get { return m_audioSource; } }
		public SoundType SoundType { get { return m_soundType; } }
		public ActivityStatus Status { get { return m_status; } }

		public SoundData CurrentSoundData { get { return m_currentSoundData; } }

		public bool IsPlaying
		{
			get
			{
				if(m_audioSource == null)
					return false;
				else
					return m_audioSource.isPlaying;
			}
		}

		/// <summary>
		/// Will this SoundSource destroy itself when the clip is done playing
		/// </summary>
		private bool m_destroyOnClipEnded = false;

		#region Callbacks

		public Action OnAudioClipEnded { get; set; }
		public Action OnFadeInCompleted { get; set; }
		public Action OnFadeOutCompleted { get; set; }

		#endregion

		#endregion

		#region Methods

		#region MonoBehaviour

		private void OnEnable()
		{
			Initialize();
		}

		protected virtual void OnDrawGizmos()
		{
			if(!Application.isPlaying)
				m_ID = name;
		}

		protected virtual void OnDestroy()
		{
			SoundManager.RemoveSoundSource(this);
		}

		private void Update()
		{
			if (m_audioSource.clip != null)
			{
				if ((m_audioSource.clip.length - m_audioSource.time) < 0.1f)
					AudioClipEnded();
			}
		}

		#endregion

		#region Behaviour

		public void Initialize()
		{
			SoundManager.AddSoundSource(this);
			m_audioSource = GetComponent<AudioSource>();
		}

		public void Initialize(SoundType type)
		{
			m_soundType = type;
			Initialize();
		}

		protected virtual void Destroy()
		{
			SoundManager.RemoveSoundSource(this);
			Destroy(gameObject);
		}

		#endregion

		#region Control Behaviour

		public void Play(SoundData data)
		{
			SetSoundData(data);

			if(m_audioSource.clip != null)
			{
				m_audioSource.Play();
				m_status = ActivityStatus.Playing;
			}
		}

		public void Play(string soundDataID)
		{
			Debug.Log($"Play Sound : {soundDataID}");
			SetSoundData(soundDataID);

			if (m_audioSource.clip != null)
			{
				m_audioSource.Play();
				m_status = ActivityStatus.Playing;
			}
		}

		public void Play()
		{
			if(m_audioSource.clip != null)
			{
				m_audioSource.Play();
				m_status = ActivityStatus.Playing;
			}
		}

		public void Pause()
		{
			if(m_audioSource.isPlaying)
			{
				m_audioSource.Pause();
				m_status = ActivityStatus.Pause;
			}
		}

		public void Stop()
		{
			m_audioSource.Stop();
			m_audioSource.clip = null;
			m_audioSource.outputAudioMixerGroup = null;
			m_status = ActivityStatus.Stop;

			if(m_destroyOnClipEnded)
				Destroy();
		}

		public void FadeInFromZero(float duration, float toVolume = 1)
		{
			m_audioSource.volume = 0;
			FadeIn(duration, toVolume);
		}

		/// <summary>
		/// Fades In the volume of the soundSource
		/// </summary>
		/// <param name="duration">Duration in seconds</param>
		/// <param name="fromZero">Volume starts from 0 ?</param>
		/// <param name="callback">Callback when done fading</param>
		public void FadeIn(float duration, float toVolume = 1)
		{
			m_status = ActivityStatus.FadeIn;

			if(!m_audioSource.isPlaying)
				m_audioSource.Play();

			m_audioSource
				.DOFade(toVolume, duration)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					OnFadeInCompleted?.Invoke();
					m_status = ActivityStatus.Playing;
				});
		}

		/// <summary>
		/// Fades out the volume of the soundSource
		/// </summary>
		/// <param name="duration">Duration in seconds</param>
		/// <param name="callback">Callback when done fading, default value fades to 0</param>
		public void FadeOut(float duration, float toVolume = 0, bool stopOnComplete = false)
		{
			m_status = ActivityStatus.FadeOut;

			m_audioSource
				.DOFade(toVolume, duration)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					OnFadeOutCompleted?.Invoke();
					m_status = ActivityStatus.Stop;

					if(toVolume == 0)
					{
						if(stopOnComplete)
							m_audioSource.Stop();
					}
				});
		}

		public void Restart()
		{
			m_audioSource.Stop();
			m_audioSource.Play();
		}

		#endregion

		#region Callbacks

		protected virtual void AudioClipEnded()
		{
			//Debug.LogFormat("AudioClipEnded : {0} \r Loop : {1}", name, m_audioSource.loop);

			if(m_audioSource.loop)
				return;

			OnAudioClipEnded?.Invoke();
			Stop();
		}

		#endregion

		#region Control Settings

		/// <summary>
		/// Set a SoundData to play
		/// </summary>
		/// <param name="data">Given SoundData</param>
		public void SetSoundData(SoundData data)
		{
			//Debug.LogFormat("SoundSource : {0} | Data : {1} ", name, data.ID);
			if(m_audioSource != null && data.Clip != null)
			{
				m_currentSoundData = data;
				m_audioSource.clip = m_currentSoundData.Clip;
				m_audioSource.outputAudioMixerGroup = m_currentSoundData.Mixer;
			}
		}

		public void SetSoundData(string ID)
		{
			SoundData data = SoundManager.GetSoundData(ID);

			if(data != null && m_audioSource != null && data.Clip != null)
			{
				m_currentSoundData = data;
				m_audioSource.clip = data.Clip;
				m_audioSource.outputAudioMixerGroup = data.Mixer;
			}
		}

		public void SetLooping(bool isLooping)
		{
			CheckAudioSource();
			m_audioSource.loop = isLooping;
		}

		public void SetVolume(float volume)
		{
			CheckAudioSource();
			m_audioSource.volume = volume;
		}

		public void SetDestroyOnClipEnded(bool destroyOnClipEnded)
		{
			m_destroyOnClipEnded = destroyOnClipEnded;
		}

		public void Unmute()
		{
			CheckAudioSource();
			m_audioSource.volume = m_volumeBeforeMute;
		}

		public void Mute()
		{
			CheckAudioSource();
			m_volumeBeforeMute = m_audioSource.volume;
			SetVolume(0);

		}

		#endregion

		private void CheckAudioSource()
		{
			if (m_audioSource == null)
				m_audioSource = GetComponent<AudioSource>();
		}

		#region From Utils

		public static bool HasFlag(int a, int b)
		{
			return (a & b) == b;
		}

		public static bool HasFlags(int a, params int[] flags)
		{
			foreach(int flag in flags)
			{
				if(HasFlag(a, flag))
					return true;
			}
			return false;
		}

		#endregion

		#endregion

		#region Debug

		public override string ToString()
		{
			string info = "";
			info += string.Format("{0} \r", name);

			if(m_currentSoundData != null)
				info += string.Format("{0} \r", m_currentSoundData.ID);

			return info;
		}

		#endregion

	}


}