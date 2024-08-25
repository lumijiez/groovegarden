using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundMusic : Singleton<BackgroundMusic>
{
    [SerializeField] private AudioClip gangnamStyle;

    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
    }

    public void Play(string sound)
    {
        switch(sound)
        {
            case "gangnam":
                if (audioSource.clip != gangnamStyle)
                {
                    audioSource.clip = gangnamStyle;
                }
                audioSource.Play();
                break;
        }
    }

    public void StopAll()
    {
        audioSource.Stop();
    }
}
