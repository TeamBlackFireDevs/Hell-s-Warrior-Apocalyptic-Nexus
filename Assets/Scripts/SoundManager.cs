using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public class EnemySounds
    {
        public string enemyID;
        public List<AudioClip> gruntingSounds = new List<AudioClip>();
        public List<AudioClip> attackSounds = new List<AudioClip>();
    }

    [SerializeField]
    public List<EnemySounds> enemySounds = new List<EnemySounds>();
    public List<AudioClip> enemyHurtSounds = new List<AudioClip>();

    public static SoundManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public AudioClip GrabASound(string enemyID, string type)
    {
        foreach(EnemySounds enemySoundsObj in enemySounds)
        {
            if(enemySoundsObj.enemyID == enemyID)
            {
                if(type == "Grunt")
                {
                    int randSound = UnityEngine.Random.Range(1,enemySoundsObj.gruntingSounds.Count);
                    AudioClip clip = enemySoundsObj.gruntingSounds[randSound];

                    enemySoundsObj.gruntingSounds[randSound] = enemySoundsObj.gruntingSounds[0];
                    enemySoundsObj.gruntingSounds[0] = clip;
                    return clip;
                }
                if(type == "Attack")
                {
                    int randSound = UnityEngine.Random.Range(1,enemySoundsObj.attackSounds.Count);
                    AudioClip clip = enemySoundsObj.attackSounds[randSound];

                    enemySoundsObj.attackSounds[randSound] = enemySoundsObj.attackSounds[0];
                    enemySoundsObj.attackSounds[0] = clip;
                    return clip;
                }
            }
        }
        return null;
    }

    public AudioClip GrabAHurtSound()
    {
        int randSound = UnityEngine.Random.Range(1,enemyHurtSounds.Count);
        AudioClip clip = enemyHurtSounds[randSound];

        enemyHurtSounds[randSound] = enemyHurtSounds[0];
        enemyHurtSounds[0] = clip;
        return clip;
    }

}
