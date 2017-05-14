using UnityEngine;
using System;

public class GodBoss : Boss {

    private static GodBoss _instance;

    /// <summary>
    /// instance unique de la classe     
    /// </summary>
    public static GodBoss instance {
        get {
            return _instance;
        }
    }

    //void Start() {

    //}

    override protected void Init() {
        Debug.Log("Hello");
        base.Init();
    }

    protected void Update() {

    }

    protected void OnDestroy() {
        _instance = null;
    }
}