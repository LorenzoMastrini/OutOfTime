using System;
using UnityEngine;
using UnityEngine.Audio;

public class MixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", (float) Math.Log10(volume) * 20f);
    }


    public void SetFXVolume(float volume)
    {
        audioMixer.SetFloat("SoundFXVolume", (float)Math.Log10(volume) * 20f);
    }


    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", (float)Math.Log10(volume) * 20f);
    }
}