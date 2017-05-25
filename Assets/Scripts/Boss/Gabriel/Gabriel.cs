using UnityEngine;
using System;
using Rpg.Characters;
using Rpg;
using Assets.Scripts.Game;

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

            HudManager.manager.bossSlider.maxValue = cara.pv;
            HudManager.manager.bossSlider.value = cara.pv;
        }
        

        private void DestroyBoss()
        {
            CinematicManager.instance.LaunchCinematic();
            //ScenesManager.instance.changeScene();
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

