using UnityEngine;
using UnityEngine.Audio;

namespace com.blackantgames.musiccontroller
{

    /// <summary>
    /// This class is responsile to manage the music in the game, transition between tracks and
    /// react to request of changes in the music system.
    /// 
    /// NOTE: This class implements a strict singleton pattern. Only one instance of this class is
    /// allowed at the same time and attempts to create new instances will result on the new instance
    /// being removed from the hierarchy
    /// </summary>
    public class MusicManager : MonoBehaviour
    {
        // Singleton instance for the music manager
        private static MusicManager INSTANCE;

        [SerializeField] private AudioSource musicSource1;
        [SerializeField] private AudioSource musicSource2;
        [SerializeField] private float musicTransitionFactor = 1;

        [SerializeField] private MusicReferenceSet musicReferenceSet;
        [SerializeField] private MusicReferenceSet deathReferenceSet;
        [SerializeField] private float defaultMusicVolume = .6f;

        private float musicVolume;
        private float songChangeTime = 0f;
        private MusicReferenceSet currentMusicSet;
        private bool muted = false;

        private enum EMusicState
        {
            SILENCE,
            AUDIO_SOURCE_1,
            AUDIO_SOURCE_2
        };
        [SerializeField] private EMusicState musicState = EMusicState.SILENCE;
        private bool updatingLevelMusic = false;

        private void Awake()
        {
            muted = PlayerPrefs.GetInt("MUTED", 0) == 1;
            if (INSTANCE == null)
            {
                INSTANCE = this;
            }
            else
            {
                Destroy(this);
            }
        }

        /// <summary>
        /// Provides access to the singleton instance reference
        /// </summary>
        /// <returns>Singleton instance of the music manager</returns>
        public static MusicManager GetInstance()
        {
            return INSTANCE;
        }

        // Start is called before the first frame update
        void Start()
        {
            musicVolume = defaultMusicVolume;
            PlayMusicFromSet(musicReferenceSet);
        }

        public void PlayDeathMusic()
        {
            PlayMusicFromSet(deathReferenceSet, false);
            currentMusicSet = null;
        }

        public void PlayInGameMusic()
        {
            muted = false;
			PlayerPrefs.SetInt("MUTED", 0);
			NPCWaveController waveController = FindObjectOfType<NPCWaveController>();
            if (waveController != null && waveController.IsWaveStarted())
            {
                currentMusicSet = waveController.GetWaveMusicReferenceSet();
            }
            else
            {
                currentMusicSet = musicReferenceSet;
            }
            PlayMusicFromAssignedSet();
        }

        public void ChangeMuteMusicState()
        {
            if (!muted) {
                musicState = EMusicState.SILENCE;
				musicSource1.volume = 0;
                musicSource2.volume = 0;
				muted = true;
				PlayerPrefs.SetInt("MUTED", 1);
			} else
            {
                musicState = EMusicState.AUDIO_SOURCE_1;
				muted = false;
				PlayerPrefs.SetInt("MUTED", 0);
			}
            updatingLevelMusic = true;
        }

        public bool GetMuteMusicState()
        {
            return muted;
        }


        /// <summary>
        /// Set the music source 1
        /// </summary>
        /// <param name="audioClip">Clip to be referenced by the AudioSource 1</param>
        private void setMusicAudio1(AudioClip audioClip, float songStartTime = 0)
        {
            musicSource1.clip = audioClip;
            musicSource1.time = songStartTime;
        }

        /// <summary>
        /// Set the music source 2
        /// </summary>
        /// <param name="audioClip">Clip to be referenced by the AudioSource 2</param>
        private void setMusicAudio2(AudioClip audioClip, float songStartTime = 0)
        {
            musicSource2.clip = audioClip;
            musicSource2.time = songStartTime;
        }

        private void FixedUpdate()
        {
            // TODO This is test logic to be removed
            if (updatingLevelMusic)
            {
                bool ms1Transition = false;
                bool ms2Transition = false;
                switch (musicState)
                {
                    case EMusicState.SILENCE:
                        ms1Transition = SilenceMusicSource(musicSource1);
                        ms2Transition = SilenceMusicSource(musicSource2);
                        break;
                    case EMusicState.AUDIO_SOURCE_1:
                        ms1Transition = ActivateMusicSource(musicSource1);
                        ms2Transition = SilenceMusicSource(musicSource2);
                        break;
                    case EMusicState.AUDIO_SOURCE_2:
                        ms1Transition = SilenceMusicSource(musicSource1);
                        ms2Transition = ActivateMusicSource(musicSource2);
                        break;
                }

                if (ms1Transition && ms2Transition)
                {
                    updatingLevelMusic = false;
                }
            }

            if (musicState.Equals(EMusicState.AUDIO_SOURCE_1))
            {
                if (musicSource1.time >= songChangeTime)
                {
                    if (currentMusicSet == null)
                    {
                        SilenceMusic();
                    }
                    else
                    {
                        PlayMusicFromSet(currentMusicSet);
                    }
                }
            }
            else if (musicState.Equals(EMusicState.AUDIO_SOURCE_2))
            {
                if (musicSource2.time >= songChangeTime)
                {
                    if (currentMusicSet == null)
                    {
                        SilenceMusic();
                    }
                    else
                    {
                        PlayMusicFromSet(currentMusicSet);
                    }
                }
            }
        }

        /// <summary>
        /// Silence the audio source
        /// </summary>
        /// <param name="musicSource">Audio source to be silenced</param>
        /// <returns></returns>
        private bool SilenceMusicSource(AudioSource musicSource)
        {
            if (musicSource.volume > 0)
            {
                musicSource.volume = musicSource.volume - (Time.deltaTime / musicTransitionFactor);
                if (musicSource.volume <= 0)
                {
                    musicSource.volume = 0;
                    musicSource.Stop();
                    return true;
                }
                return false;
            }
            else
            {
                musicSource.Stop();
                return true;
            }
        }

        /// <summary>
        /// Activate a specific audio source
        /// </summary>
        /// <param name="musicSource"></param>
        /// <returns></returns>
        private bool ActivateMusicSource(AudioSource musicSource)
        {
            if (musicSource.volume < musicVolume)
            {
                if (!musicSource.isPlaying)
                {
                    musicSource.Play();
                }

                musicSource.volume += (Time.deltaTime / musicTransitionFactor);
                if (musicSource.volume > musicVolume)
                {
                    musicSource.volume = musicVolume;
                    return true;
                }
            }
            else
            {
                musicSource.volume -= (Time.deltaTime / musicTransitionFactor);
                if (musicSource.volume < musicVolume)
                {
                    musicSource.volume = musicVolume;
                    if (!musicSource.isPlaying)
                    {
                        musicSource.Play();
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Changes the music source being played
        /// </summary>
        private void ChangeMusicSource(bool silence = false)
        {
            if (silence)
            {
                musicState = EMusicState.SILENCE;
                updatingLevelMusic = true;
            }
            else if (musicState.Equals(EMusicState.SILENCE) || musicState.Equals(EMusicState.AUDIO_SOURCE_2))
            {
                musicState = EMusicState.AUDIO_SOURCE_1;
                updatingLevelMusic = true;
            }
            else
            {
                musicState = EMusicState.AUDIO_SOURCE_2;
                updatingLevelMusic = true;
            }
        }

        /// <summary>
        /// Play the music for a specific music set
        /// </summary>
        /// <param name="referenceSet"></param>
        public void PlayMusicFromSet(MusicReferenceSet referenceSet, bool assignSet = true)
        {
            if (referenceSet != null)
            {
                if (assignSet)
                {
                    currentMusicSet = referenceSet;
                }
                SongPlayProperties songProperties = referenceSet.GetSong();
                musicVolume = songProperties.GetSongExpectedVolume();
                songChangeTime = songProperties.GetTimeToFinishSong();
                if (!muted)
                {
                    ChangeMusic(songProperties.GetAudioClip(), songProperties.GetStartTime());
                } else
				{
                    setMusicAudio1(songProperties.GetAudioClip(), songProperties.GetStartTime());
                    setMusicAudio2(songProperties.GetAudioClip(), songProperties.GetStartTime());
				}
			}
            else
            {
                SilenceMusic();
			}
        }

        /// <summary>
        /// Play a specific song provided through it's song properties
        /// </summary>
        /// <param name="songProperties">Song properties to play</param>
        public void PlayMusicClip(SongPlayProperties songProperties)
        {
            musicVolume = songProperties.GetSongExpectedVolume();
            songChangeTime = songProperties.GetTimeToFinishSong();
            ChangeMusic(songProperties.GetAudioClip(), songProperties.GetStartTime());
        }

        /// <summary>
        /// Play other song from the assigned current set
        /// </summary>
        public void PlayMusicFromAssignedSet()
        {
            PlayMusicFromSet(currentMusicSet);
        }

        /// <summary>
        /// Change the music to be played in the game
        /// </summary>
        /// <param name="clip">Clip to be played</param>
        private void ChangeMusic(AudioClip clip, float songStartTime = 0f)
        {
            switch (musicState)
            {
                case EMusicState.SILENCE:
                case EMusicState.AUDIO_SOURCE_2:
                    setMusicAudio1(clip, songStartTime);
                    break;
                case EMusicState.AUDIO_SOURCE_1:
                    setMusicAudio2(clip, songStartTime);
                    break;
            }
            ChangeMusicSource();
        }

        /// <summary>
        /// Silence the music
        /// </summary>
        public void SilenceMusic()
        {
            currentMusicSet = null;
            ChangeMusicSource(true);
        }


        /// <summary>
        /// Provides the current music reference set
        /// </summary>
        /// <returns>Current music reference set</returns>
        public MusicReferenceSet GetCurrentMusicSet()
        {
            return currentMusicSet;
        }
    }
}