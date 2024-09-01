using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    AudioSource _AudioSource;

    [SerializeField]
    List<AudioClip> _audioClipList;

    void Start()
    {
        GetComponent<AudioSource>();
    }

    /// <summary>
    /// AudioListのサウンドを再生する
    /// </summary>
    /// <param name="SoundNumber">AudioListのElementのIndexを参照します</param>
    public void PlaySE(int SoundNumber)
    {
        if (_AudioSource != null && _audioClipList[SoundNumber] != null)
        {
            _AudioSource.PlayOneShot(_audioClipList[SoundNumber]);
        }
    }
}
