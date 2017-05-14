using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour {

    public float velocity;

    // Update is called once per frame
    void FixedUpdate() {
        GetComponent<Rigidbody>().MovePosition(transform.position + Time.fixedDeltaTime * velocity * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
    }
}