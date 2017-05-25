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

    protected void Update() {

    }

    protected void OnDestroy() {
        _instance = null;
    }
}