using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationFinish : MonoBehaviour {

    public UnityEvent finish;

    private static AnimationFinish _instance;

    /// <summary>
    /// instance unique de la classe     
    /// </summary>
    public static AnimationFinish instance {
        get {
            return _instance;
        }
    }

    protected void Awake() {
        _instance = this;

        finish = new UnityEvent();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void doEvent() {
        finish.Invoke();
    }
}
