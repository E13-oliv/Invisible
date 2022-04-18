using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Mixers")]
    [SerializeField]
    private AudioMixer globalMixer;
    [SerializeField]
    private AudioMixer musicMixer;
    [SerializeField]
    private AudioMixer sfxMixer;
    [SerializeField]
    private AudioMixer heartBeatMixer;

    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource titleAudioSource;
    [SerializeField]
    private AudioSource levelAudioSource;

    [Header("Title Snapshot")]
    [SerializeField]
    private AudioMixerSnapshot titleSnapshot;
    [SerializeField]
    private AudioMixerSnapshot levelSnapshot;

    [Header("HeartBeat Snapshot")]
    [SerializeField]
    private AudioMixerSnapshot heartBeatOffSnapshot;
    [SerializeField]
    private AudioMixerSnapshot heartBeatOnSnapshot;

    private static float crossFadeDuration = 1.0f;

    private static float heartBeatMinPitch = .5f;
    private static float heartBeatMaxPitch = 1.0f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            BackToTitleAudio();
        }
    }

    public void SetVolume(string param, float volume)
    {
        // volume conversion to match log formula
        if (volume == 0)
        {
            volume = 0.001f;
        }
        else
        {
            volume = volume / 5;
        }

        float newVolume = Mathf.Log(volume) * 20;
        globalMixer.SetFloat(param, newVolume);
    }

    public void LevelStartAudio(AudioClip levelMusic)
    {
        levelAudioSource.clip = levelMusic;
        levelAudioSource.Play();

        levelSnapshot.TransitionTo(crossFadeDuration);
    }

    public void BackToTitleAudio()
    {
        titleSnapshot.TransitionTo(crossFadeDuration);
        StartCoroutine(RemoveAudioClipCoroutine());
    }

    private IEnumerator RemoveAudioClipCoroutine()
    {
        yield return new  WaitForSecondsRealtime(crossFadeDuration);
        levelAudioSource.clip = null;
    }

    public void SetHeartBeat(float factor)
    {

        if (factor == 0)
        {
            heartBeatOffSnapshot.TransitionTo(crossFadeDuration);
        }
        else
        {
            heartBeatOnSnapshot.TransitionTo(crossFadeDuration);
        }

        float pitch = Mathf.Lerp(heartBeatMinPitch, heartBeatMaxPitch, factor);

        heartBeatMixer.SetFloat("HeartBeatPitch", pitch);
    }
}
