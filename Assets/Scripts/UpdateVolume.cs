using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateVolume : MonoBehaviour
{

    public bool isMusic = false;
    void Start()
    {
        if(isMusic)
        {
            GetComponent<AudioSource>().volume = GlobalVolumeManager.Instance.GetMusicVol();
        }else
        {
            GetComponent<AudioSource>().volume = GlobalVolumeManager.Instance.GetSoundVol();
        }
        GlobalVolumeManager.Instance.onUpdatedVolume.AddListener(UpdateTheVolume);
    }

    public void UpdateTheVolume()
    {
        if(isMusic)
        {
            GetComponent<AudioSource>().volume = GlobalVolumeManager.Instance.GetMusicVol();
        }else
        {
            GetComponent<AudioSource>().volume = GlobalVolumeManager.Instance.GetSoundVol();
        }
    }


}
