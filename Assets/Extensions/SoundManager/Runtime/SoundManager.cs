using System.Collections.Generic;
using UnityEngine;

namespace VirtuoseReality.Extension.AudioManager
{

	/// <summary>
	/// Control class
	/// </summary>
	public static class SoundManager
	{

		#region Fields

		#region Behaviour

		public static bool m_isInitialized = false;

		#endregion

		#region SoundSources

		public static Transform soundSourceContainer = null;

		private static Dictionary<SoundType, List<SoundSource>> m_typeToSoundSources = new Dictionary<SoundType, List<SoundSource>>();

		public static AudioMixerController masterAudioMixer
		{
			get
			{
				return settings.AudioMixerControllers.Find(mixer => mixer.Name == "Master");
			}
		}

		#endregion

		#region SoundLibraries

		public static SoundManagerToolSettings settings = null;

		private static List<SoundDataLibrary> SoundDataLibraries { get { return settings.SoundDataLibraries; } }

		#endregion

		#region Global Settings

		private static float BaseFadeInDuration { get { return settings.BaseFadeInDuration; } }
		private static float BaseFadeOutDuration { get { return settings.BaseFadeOutDuration; } }

		private static readonly float UNMUTE_VOLUME = 0f;
		private static readonly float MUTE_VOLUME = -80f;
		public static readonly string k_soundManagerToolSettings = "SoundManagerToolSettings";

		#endregion

		#endregion

		#region Methods

		#region Behaviour

		/// <summary>
		/// Initialize the AudioManager
		/// </summary>
		public static void Init(string pathToSettings)
		{
			/// Settings
			settings = Resources.Load(pathToSettings + "/" + k_soundManagerToolSettings) as SoundManagerToolSettings;

			if (settings == null)
			{
				Debug.LogErrorFormat("SoundManagerTool is not properly initialized");
				return;
			}

			/// SoundDataLibraries
			foreach (SoundDataLibrary library in SoundDataLibraries)
				library.Init();

			m_isInitialized = true;

			Debug.Log("SoundManagerTool is initialized !");
		}

		#endregion

		#region Control


		///// <summary>
		///// Play a sound identified by it's ID
		///// </summary>
		///// <param name="soundDataID"> ID of the sound</param>
		///// <returns>SoundSource for specific behaviour</returns>
		//public static SoundSource PlaySound(string soundDataID, bool isLooping = false)
		//{
		//	if(!m_isInitialized)
		//		return null;

		//	Debug.Log("Play Sound");

		//	// Check if all libraries has this ID stored
		//	SoundData soundData = GetSoundData(soundDataID);
		//	if(soundData == null)
		//	{
		//		Debug.LogError(string.Format("Can't play SoundData with ID {0}, because it's null", soundDataID));
		//		return null;
		//	}
		//	//Debug.Log(soundData.Clip.length);

		//	// Find a non playing SoundSource to play the sound
		//	SoundSource source = GetNonPlayingSoundSource(soundData.Type);
		//	source.SetLooping(isLooping);

		//	source.Play(soundData);

		//	return source;
		//}

		/// <summary>
		/// Play a sound identified by it's ID
		/// </summary>
		/// <param name="soundDataID"> ID of the sound</param>
		/// <param name="fadeDuration">FadeDuration. Not changing the value means it will use AAudioManager.m_fadeInBaseDuration</param>
		/// <returns>SoundSource for specific behaviour</returns>
		public static SoundSource PlaySound(string soundDataID, bool isLooping = false, float fadeDuration = -1f)
		{

			if(!m_isInitialized)
				return null;
			
			// Debug.Log("Play : " + soundDataID);

			// Check if all libraries has this ID stored
			SoundData soundData = GetSoundData(soundDataID);
			if(soundData == null)
			{
				Debug.LogError(string.Format("Can't play SoundData with ID {0}, because it's null", soundDataID));
				return null;
			}

			// Find a non playing SoundSource to play the sound
			SoundSource source = GetNonPlayingSoundSource(soundData.Type);
			source.SetSoundData(soundData);
			source.SetLooping(isLooping);

			if(fadeDuration == -1f)
				fadeDuration = BaseFadeInDuration;

			source.FadeInFromZero(fadeDuration);

			return source;
		}

		//public static void StopSound(string soundDataID)
		//{

		//	if(!m_isInitialized)
		//		return;

		//	List<SoundSource> playingSources = SoundSourcesPlayingSoundID(soundDataID);

		//	foreach(SoundSource source in playingSources)
		//		source.Stop();
		//}

		public static void StopSound(string soundDataID, float fadeDuration = -1f)
		{
			if(!m_isInitialized)
				return;

			List<SoundSource> playingSources = SoundSourcesPlayingSoundID(soundDataID);

			if(fadeDuration == -1f)
				fadeDuration = BaseFadeOutDuration;

			foreach(SoundSource source in playingSources)
				source.FadeOut(fadeDuration);
		}

		//public static void BlendSound(string IDFadeOut, string IDFadeIn, float blendDuration)
		//{
		//	StopSound(IDFadeOut, blendDuration);
		//	PlaySound(IDFadeIn,false blendDuration);
		//}

		//public static void BlendSound(string IDFadeOut, string IDFadeIn, float durationFadeIn, float durationFadeOut)
		//{
		//	StopSound(IDFadeOut, durationFadeOut);
		//	PlaySound(IDFadeIn, durationFadeIn);
		//}

		#endregion

		#region SoundData

		/// <summary>
		/// Get the SoundData with given ID
		/// </summary>
		/// <param name="ID"> ID of the SoundData</param>
		/// <returns></returns>
		public static SoundData GetSoundData(string ID)
		{
			if(!m_isInitialized)
				return null;

			SoundData sound = null;

			foreach(SoundDataLibrary library in SoundDataLibraries)
			{
				sound = library.SoundDatas.Find(data => data.ID == ID);

				if(sound != null)
					return sound;
			}

			Debug.LogError(string.Format("There is no SoundData with ID {0} in libraries", ID));
			return null;
		}

		#endregion

		#region SoundSource

		/// <summary>
		/// Dynamicaly retrieve the last available SoundSource. If there is none availble, creates a new one and stores it
		/// </summary>
		/// <param name="soundSources"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static SoundSource GetNonPlayingSoundSource(SoundType type)
		{
			// First, we find the list of SoundSources of given type
			List<SoundSource> soundSources = GetSoundSources(type);
			SoundSource soundSource = null;

			// if there is none, we create a new one and store it
			if(soundSources.Count == 0)
				soundSource = CreateSoundSource(type);
			else
			{
				// We find the SoundSource currently available to play something
				soundSource = soundSources.Find(source => !source.IsPlaying);

				// If thre is no SoundSource available, we create a new one and store it
				if(soundSource == null)
					soundSource = CreateSoundSource(type);
			}

			return soundSource;
		}

		/// <summary>
		/// Get the list of SoundSource of given type
		/// </summary>
		/// <param name="type">SoundType of SoundSource</param>
		/// <returns></returns>
		public static List<SoundSource> GetSoundSources(SoundType type)
		{
			// If there is no SoundSource stored of this type, creates a new list, store it and a new SoundSource
			if(!m_typeToSoundSources.ContainsKey(type))
			{
				List<SoundSource> soundSources = new List<SoundSource>();
				m_typeToSoundSources.Add(type, soundSources);
				CreateSoundSource(type);

				return soundSources;
			}
			else
				return m_typeToSoundSources[type];
		}

		/// <summary>
		/// Create a new SoundSource and store it
		/// </summary>
		/// <param name="type">SoundType of SoundSource</param>
		/// <returns></returns>
		public static SoundSource CreateSoundSource(SoundType type)
		{

			GameObject newObject = new GameObject();
			newObject.transform.parent = soundSourceContainer;
			newObject.AddComponent<AudioSource>();

			SoundSource newSource = newObject.AddComponent<SoundSource>();
			newSource.name = string.Format("{0}_{1}", type.ToString(), GetSoundSources(type).Count);
			newSource.Initialize(type);
			newSource.SetDestroyOnClipEnded(true);

			return newSource;
		}

		/// <summary>
		/// Remove a stored SoundSource
		/// </summary>
		/// <param name="soundSource">Instance to remove</param>
		public static void RemoveSoundSource(SoundSource soundSource)
		{
			foreach(KeyValuePair<SoundType, List<SoundSource>> pair in m_typeToSoundSources)
				if(pair.Value.Contains(soundSource))
					pair.Value.Remove(soundSource);
		}

		/// <summary>
		/// Store a SoundSource according to its type, to mute it maybe
		/// </summary>
		/// <param name="soundSource"></param>
		public static void AddSoundSource(SoundSource soundSource)
		{
			if(m_typeToSoundSources.ContainsKey(soundSource.SoundType))
				m_typeToSoundSources[soundSource.SoundType].Add(soundSource);
			else
				m_typeToSoundSources.Add(soundSource.SoundType, new List<SoundSource>() { soundSource });
		}

		/// <summary>
		/// Retrieve all SoundSources playing a specific sound, identified by its SoundID
		/// </summary>
		/// <param name="soundID">ID of the sound</param>
		/// <returns></returns>
		public static List<SoundSource> SoundSourcesPlayingSoundID(string soundID)
		{
			List<SoundSource> sources = new List<SoundSource>();

			foreach(KeyValuePair<SoundType, List<SoundSource>> pair in m_typeToSoundSources)
			{
				sources.AddRange(pair.Value.FindAll(source => source.CurrentSoundData != null && source.CurrentSoundData.ID == soundID));
			}

			return sources;
		}

		#endregion

		#region Mute / Unmute

		/// <summary>
		/// Mute or Unmute SoundType, linked to AudioMixerController
		/// </summary>
		/// <param name="type"></param>
		/// <param name="muteOrUnmute"></param>
		public static void MuteUnmuteSoundType(SoundType type, bool muteOrUnmute)
		{
			AudioMixerController mixer = MixerControllerFromSoundType(type);

			if(mixer != null)
				mixer.SetMasterVolume(muteOrUnmute ? MUTE_VOLUME : UNMUTE_VOLUME);
		}

		private static AudioMixerController MixerControllerFromSoundType(SoundType type)
		{
			return settings.AudioMixerControllers.Find(mixer => mixer.Type == type);
		}


		#endregion

		#region Debug

		public static void Debug_DebugInfos()
		{
			foreach(KeyValuePair<SoundType, List<SoundSource>> pair in m_typeToSoundSources)
			{
				Debug.Log("////////////////");
				Debug.LogFormat("{0}", pair.Key);

				foreach(SoundSource source in pair.Value)
					Debug.LogFormat("{0}", source.ToString());
			}
		}

		#endregion

		#endregion

	}

}
