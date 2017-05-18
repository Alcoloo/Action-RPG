using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Assets.Scripts.Manager;
using Rpg;

public class IntroManager : MonoBehaviour {
    
    public GameObject[] introArray;
    private int currentCinematiqueIndex;

    private float switchTimer = 2.0f;
    private bool isSwitching = false;

	// Use this for initialization
	void Start () {
        currentCinematiqueIndex = 0;
        ChangeImageAlpha();
	}
	
	// Update is called once per frame
	void Update () {
        switchTimer -= Time.deltaTime;
        if (isCinematiqueOver())
        {
            Debug.Log("go");
            StopCoroutine(SetNextOneActive());
            ScenesManager.manager.changeScene();
        }
        if (switchTimer <= 0.0f && !isSwitching)
        {
            isSwitching = true;
            StartCoroutine(SetNextOneActive());
        }
	}

    private void ChangeImageAlpha()
    {
        for (int i = 1; i < introArray.Length; i++)
        {
            introArray[i].GetComponent<CanvasRenderer>().SetAlpha(0.01f);
        }
    }

    IEnumerator SetNextOneActive()
    {
        introArray[currentCinematiqueIndex].GetComponent<Image>().CrossFadeAlpha(0.0f, 2.5f, false);
        yield return new WaitForSeconds(0.5f);

        introArray[currentCinematiqueIndex].SetActive(false);
        introArray[++currentCinematiqueIndex].GetComponent<Image>().CrossFadeAlpha(1.0f, 2.5f, false);
        yield return new WaitForSeconds(0.5f);

        switchTimer = 2.0f;
        isSwitching = false;

        yield return null;
    }

    private bool isCinematiqueOver()
    {
        if (currentCinematiqueIndex == introArray.Length -1) return true;
        else return false;
    }
}
