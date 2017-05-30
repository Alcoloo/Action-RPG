using Rpg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Player>()) Player.instance.SetModeDeath();
    }
}
