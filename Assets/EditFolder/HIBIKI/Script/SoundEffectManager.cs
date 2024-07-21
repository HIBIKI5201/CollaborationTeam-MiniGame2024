using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    static AudioSource _AudioSource;

    [SerializeField]
    List<AudioClip> _audioClipList;

    static List<AudioClip> _staticAudio;

    void Start()
    {
        _AudioSource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        _staticAudio = _audioClipList;
    }

    public static void PlaySE(int SoundNumber)
    {
        _AudioSource.PlayOneShot(_staticAudio[SoundNumber]);
    }
}
