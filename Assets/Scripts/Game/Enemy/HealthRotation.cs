using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRotation : MonoBehaviour {

    protected Transform cam;
    protected Transform lifebar;
    protected Vector3 scale;
    // Use this for initialization

    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        lifebar = transform.FindChild("HealthBar");
    }

    void Start ()
    {
        scale = lifebar.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(cam);
	}

    public void changeLife(float pPercentLife)
    {
        lifebar.localScale = new Vector3(pPercentLife, 1, 1);
    }
}
