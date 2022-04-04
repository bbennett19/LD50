using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer Instance { get; private set; }

    [SerializeField]
    private AudioSource _audioSource;

    private AudioClip _lastClip;
    private float playTime;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayAudio(AudioClip clip, bool trackClip = false)
    {
        Debug.Log(Time.time - playTime);
        if (trackClip && (_audioSource.isPlaying || Time.time - playTime < 1.3f) && clip == _lastClip)
            return;

        if (trackClip)
        {
            playTime = Time.time;
            _lastClip = clip;
        }
        _audioSource.PlayOneShot(clip);
    }

}
