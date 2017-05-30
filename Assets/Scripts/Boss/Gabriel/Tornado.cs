using UnityEngine;
using System;
using System.Collections;
using Rpg;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class Tornado : MonoBehaviour
    {
        public float speed;
        public float destructionTime;

        private float startDestructionTime;

        protected void Awake()
        {
            DontDestroyOnLoad(transform.root.gameObject);
        }

        protected void Start()
        {
            startDestructionTime = CustomTimer.manager.elapsedTime;
        }

        protected void Update()
        {
            transform.Translate(Vector3.forward * speed);
            if (CustomTimer.manager.isTime(startDestructionTime, destructionTime)) gameObject.SetActive(false);
        }

        
        
    }
}