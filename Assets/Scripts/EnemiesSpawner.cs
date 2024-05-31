using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{

    [Serializable]
    public class WaveProperties
    {
        public float minSpawnTime;
        public float maxSpawnTime;

        public int minBurstRate;
        public int maxBurstRate;

        public float waveDuration;

        public List<GameObject> enemies = new List<GameObject>();
    
    }
    public List<Transform> spawnPoints = new List<Transform>();
    [SerializeField]
    public List<WaveProperties> waves = new List<WaveProperties>();

    public TMPro.TMP_Text waveText;
    public GameObject waveUIObj;

    float spawnTimer, randTime;
    float waveTimer;
    int waveNo = 0;
    
    bool waitingForSpawn;

    void Start()
    {
        ShowWaveText();
    }

    void Update()
    {
        if(!waitingForSpawn)
        {
            waitingForSpawn = true;
            spawnTimer = 0f;
            randTime = UnityEngine.Random.Range(waves[waveNo].minSpawnTime,waves[waveNo].maxSpawnTime);
        }else
        {
            spawnTimer += Time.deltaTime;

            if(spawnTimer >= randTime)
            {
                SpawnEnemy();
                waitingForSpawn = false;
            }

        }
        waveTimer += Time.deltaTime;
        if(waveTimer >= waves[waveNo].waveDuration)
        {
            waveNo ++;
            waveTimer = 0f;
            waitingForSpawn = false;
            ShowWaveText();
        }
    }

    void ShowWaveText()
    {
        waveUIObj.SetActive(false);
        waveText.text = "Wave " + (waveNo + 1).ToString();
        waveUIObj.SetActive(true);
    }

    void SpawnEnemy()
    {
        int randBurstAmt = UnityEngine.Random.Range(waves[waveNo].minBurstRate,waves[waveNo].maxBurstRate);

        for (int i = 0; i < randBurstAmt; i++)
        {
            Transform randPoint = spawnPoints[UnityEngine.Random.Range(0,spawnPoints.Count)];
            GameObject randEnemy = waves[waveNo].enemies[UnityEngine.Random.Range(0,waves[waveNo].enemies.Count)];

            Instantiate(randEnemy,randPoint.position,Quaternion.identity);
        }
    }
}
