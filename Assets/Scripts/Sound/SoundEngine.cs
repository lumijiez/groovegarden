using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEngine : Singleton<SoundEngine>
{
    [SerializeField] private AudioClip bowSound;
    [SerializeField] private AudioClip swordSound;
    [SerializeField] private AudioClip magicSound;

    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
    }

    public void Play(string sound)
    {
        switch (sound)
        {
            case "bow":
                if (audioSource.clip != bowSound)
                {
                    audioSource.clip = bowSound;
                }
                audioSource.Play();
                break;
            case "sword":
                if (audioSource.clip != swordSound)
                {
                    audioSource.clip = swordSound;
                }
                audioSource.Play();
                break;
            case "magic":
                if (audioSource.clip != magicSound)
                {
                    audioSource.clip = magicSound;
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
