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
        

        if (GameObject.Find("SoundManager").TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            _AudioSource = audioSource;
        }
        else
        {
            Debug.LogError("SoundManagerオブジェクトを作り、AudioSourceをアタッチしてください");
        }

        _staticAudio = _audioClipList;
    }

    public static void PlaySE(int SoundNumber)
    {
        _AudioSource.PlayOneShot(_staticAudio[SoundNumber]);
    }
}
