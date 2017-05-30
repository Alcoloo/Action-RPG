using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTrigger : MonoBehaviour {
    
    void OnTriggerEnter(Collider col)
    {
        CinematicManager.instance.LaunchCinematic();
        Destroy(gameObject);
    }
}
