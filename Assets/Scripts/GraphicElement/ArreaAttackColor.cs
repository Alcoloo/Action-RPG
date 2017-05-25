using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArreaAttackColor : MonoBehaviour {

    public float minAlpha = 0;
    public float maxAlpha = 1;

    public float deltaAlpha = 1;

    private bool increaseAlpha = false;

    private Color color;
    private Color colorA;
    private Color colorB;

	// Use this for initialization
	void Start () {
        color = gameObject.GetComponent<Renderer>().material.color;
        color = new Vector4(color.r, color.g, color.b, maxAlpha);

        colorA = new Vector4(color.r, color.g, color.b, maxAlpha);
        colorB = new Vector4(color.r, color.g, color.b, minAlpha);
    }
	
	// Update is called once per frame
	void Update () {

        color = gameObject.GetComponent<Renderer>().material.color;

        if (increaseAlpha) color.a += deltaAlpha;
        else color.a -= deltaAlpha;

        gameObject.GetComponent<Renderer>().material.color = color;

        if (color.a >= maxAlpha) increaseAlpha = false;
        else if (color.a <= minAlpha) increaseAlpha = true;

    }
}
