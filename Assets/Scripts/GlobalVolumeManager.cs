using System.Collections;
using System.Collections.Generic;
using HQFPSWeapons;
using UnityEngine;
using UnityEngine.Events;

public class GlobalVolumeManager : MonoBehaviour
{
    float soundVol;
    float musicVol;

    public static GlobalVolumeManager Instance;
    public UnityEvent onUpdatedVolume = new UnityEvent();
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }

        

        soundVol = PlayerPrefs.GetFloat("SoundVolume",1f);
        musicVol = PlayerPrefs.GetFloat("MusicVolume",1f);
    }



    public void SetSoundVol(float volume)
    {
        soundVol = volume;
        PlayerPrefs.SetFloat("SoundVolume",volume);
        onUpdatedVolume.Invoke();
    }

    public void SetMusicVol(float volume)
    {
        musicVol = volume;
        PlayerPrefs.SetFloat("MusicVolume",volume);
        onUpdatedVolume.Invoke();
    }

    public float GetSoundVol()
    {
        return soundVol;
    }

    public float GetMusicVol()
    {
        return musicVol;
    }
}
