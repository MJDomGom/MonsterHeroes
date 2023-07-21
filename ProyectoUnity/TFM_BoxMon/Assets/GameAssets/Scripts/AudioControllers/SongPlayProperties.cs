using UnityEngine;

namespace com.blackantgames.musiccontroller
{

	/// <summary>
	/// Class that represents the music track properties to be used by the Music Manager
	/// </summary>
	[CreateAssetMenu(fileName = "SongProperties", menuName = "MusicReference/SongProperties")]
	public class SongPlayProperties : ScriptableObject
	{
		[SerializeField] private AudioClip song;
		[SerializeField] private float songStartTime = 0f;
		[SerializeField] private float songExpectedVolume = 0.6f;
		[SerializeField] private float timeToFinishSong = 0f;

		/// <summary>
		/// Provides the audio clip for the specific song
		/// </summary>
		/// <returns>Song audio clip</returns>
		public AudioClip GetAudioClip()
		{
			return song;
		}

		/// <summary>
		/// Provides the start time to start playing the indicated song
		/// </summary>
		/// <returns>Float start time</returns>
		public float GetStartTime()
		{
			return songStartTime;
		}

		/// <summary>
		/// Provides the expected volume at which the song should be played
		/// </summary>
		/// <returns>Expected song volume</returns>
		public float GetSongExpectedVolume()
		{
			return songExpectedVolume;
		}

		/// <summary>
		/// Indicates when is expected the song to be changed
		/// </summary>
		/// <returns>Time when the song is expected to be changed</returns>
		public float GetTimeToFinishSong()
		{
			// If not set, or set not correcly the end time should be .5 seconds before the end of the song
			if (timeToFinishSong < 0.1f || timeToFinishSong < songStartTime)
			{
				return song.length - .5f;
			}
			else
			{
				// Otherwise, the song must finish at the indicated time
				return timeToFinishSong;
			}
		}
	}
}