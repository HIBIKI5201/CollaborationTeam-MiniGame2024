using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
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
        if (_AudioSource != null)
        {
            if (_audioClipList[SoundNumber] != null)
            {
                _AudioSource.PlayOneShot(_audioClipList[SoundNumber]);
            }
        }
        else Debug.LogWarning("SoundEffectManagerにAudioSourceがアタッチされていません");
    }
}
