using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Assets.Scripts.Manager;
using Rpg;

public class IntroManager : MonoBehaviour {
    
    public GameObject[] introArray;
    private int currentCinematiqueIndex;
    private IEnumerator activeCoroutine;

    private float switchTimer = 2.0f;
    private bool isSwitching = false;

	// Use this for initialization
	void Start () {
        currentCinematiqueIndex = 0;
        ChangeImageAlpha();
        activeCoroutine = SetNextOneActive();
        StartCoroutine(activeCoroutine);

    }
	
	// Update is called once per frame
	void Update () {
        isCinematiqueOver();
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
        while(currentCinematiqueIndex <= introArray.Length-1)
        {
            Debug.Log("a");
            introArray[currentCinematiqueIndex].GetComponent<Image>().CrossFadeAlpha(0.0f, 2.5f, false);
            yield return new WaitForSeconds(0.5f);
            Debug.Log("c");
            introArray[currentCinematiqueIndex].SetActive(false);

            Debug.Log("b");
            introArray[++currentCinematiqueIndex].GetComponent<Image>().CrossFadeAlpha(1.0f, 2.5f, false);
            yield return new WaitForSeconds(0.5f);
            yield return null;
        }
    }

    private void isCinematiqueOver()
    {
        if (currentCinematiqueIndex == introArray.Length -1 && !isSwitching)
        {
            StopCoroutine(activeCoroutine);
            ScenesManager.manager.changeScene();
            isSwitching = true;
        }
    }
}
