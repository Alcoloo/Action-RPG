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
    public class Gabriel : Boss
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

        protected override void Awake()
        {
            base.Awake();
            if (_instance != null)
            {
                throw new Exception("Tentative de création d'une autre instance de Gabriel alors que c'est un singleton.");
            }
            _instance = this;

            hasBeenhit = false;
            
        }

        public override void Start()
        {
            base.Start();
            HudManager.manager.bossSlider.maxValue = health;
            HudManager.manager.bossSlider.value = health;
        }


        protected void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.name == "Player")
            {
                hasBeenhit = true;
            }
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
        }

        public override void UpdateHealthBar()
        {
            HudManager.manager.ChangeBossLifeValue(health);
        }

        protected override void SetModeDie()
        {
            DoActionDie();
        }

        protected override void DoActionDie()
        {
            CinematicManager.instance.LaunchCinematic();
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

