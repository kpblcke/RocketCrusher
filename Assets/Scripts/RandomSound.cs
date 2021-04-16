using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RSound", menuName = "SoundList", order = 0)]
public class RandomSound : ScriptableObject
{
    [SerializeField]
    private List<AudioClip> _audioClips;

    public AudioClip GetSound()
    {
        return _audioClips[Random.Range(0, _audioClips.Count)];
    }
}
