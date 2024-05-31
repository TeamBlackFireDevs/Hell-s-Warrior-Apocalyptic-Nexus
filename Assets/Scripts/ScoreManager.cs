using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public TMPro.TMP_Text scoreText;
    int hellPoints;

    public static ScoreManager Instance;

    public UnityEvent<int> earnedHP = new UnityEvent<int>();

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }

    public int GetHellPoints()
    {
        return hellPoints;
    }

    public void AddPoints(int points)
    {
        hellPoints += points;
        scoreText.text = hellPoints.ToString();
        earnedHP.Invoke(points);
    }

    public void SubPoints(int points)
    {
        hellPoints -= points;
        scoreText.text = hellPoints.ToString();
    }

    
}
