using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg {

    /// <summary>
    /// 
    /// </summary>
    public class fixPositionY : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y), transform.position.z);
        }
    }
}