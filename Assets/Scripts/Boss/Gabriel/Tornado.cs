using UnityEngine;
using System;
using System.Collections;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class Tornado : MonoBehaviour
    {
        public float speed;
        
        

        protected void Start()
        {
            StartCoroutine(MoveForward());
        }

        protected void Update()
        {
            
            
        }

        IEnumerator MoveForward()
        {
            while(gameObject != null)
            {
                transform.Translate(Vector3.forward * speed);
                Destroy(gameObject, 3.0f);
                yield return null;
            }
        }
    }
}