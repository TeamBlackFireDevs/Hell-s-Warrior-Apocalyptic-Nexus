using System.Collections;
using System.Collections.Generic;
using HQFPSWeapons;
using UnityEngine;

public class DeathOnCollide : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.GetComponent<Player>() != null)
        {
            GameManager.Instance.CurrentPlayer.ChangeHealth.Try(new HealthEventData(-120));
        }
    }
}
