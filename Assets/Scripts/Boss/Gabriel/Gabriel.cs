using UnityEngine;
using System;
using Rpg.Characters;
using Rpg;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class Gabriel : MonoBehaviour
    {
        private Caracteristic cara;
        private bool hasBeenhit;

        private static Gabriel _instance;

        /// <summary>
        /// instance unique de la classe     
        /// </summary>
        public static Gabriel instance
        {
            get
            {
                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance != null)
            {
                throw new Exception("Tentative de création d'une autre instance de Gabriel alors que c'est un singleton.");
            }
            _instance = this;

            hasBeenhit = false;
            cara = GetComponent<Caracteristic>();
        }

        protected void Start()
        {
            if (cara.isDeath != null) cara.isDeath.AddListener(DestroyBoss);
        }
        

        private void DestroyBoss()
        {
            Destroy(gameObject);
            ScenesManager.instance.changeScene();
        }  
        
        protected void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.name == "Sword") hasBeenhit = true;
        }

        protected void OnCollisionExit(Collision col)
        {
            hasBeenhit = false;
        }

        public bool HasBeenHit()
        {
            return hasBeenhit;
        }

        protected void OnDestroy()
        {
            _instance = null;
        }
    }
}

