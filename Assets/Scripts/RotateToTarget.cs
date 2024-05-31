using System.Collections;
using System.Collections.Generic;
using HQFPSWeapons;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    // public Transform target;

    // public float turnSpeed;
    // void Start()
    // {
    //     target = GameManager.Instance.CurrentPlayer.transform;
    // }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
