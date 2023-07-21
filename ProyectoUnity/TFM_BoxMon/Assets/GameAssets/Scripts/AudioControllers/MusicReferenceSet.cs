using UnityEngine;

namespace com.blackantgames.musiccontroller
{
	/// <summary>
	/// Represents a set of audio clips that should not be played as a set such as calm music sets,
	/// combat music sets, etc
	/// </summary>
	[CreateAssetMenu(fileName = "MusicReference", menuName = "MusicReference/MusicClipReferenceSet")]
	public class MusicReferenceSet : ScriptableObject
	{
		[SerializeField] private SongPlayProperties[] musicSet;

		public SongPlayProperties GetSong()
		{
			return musicSet[Random.Range(0, musicSet.Length)];
		}
	}
}