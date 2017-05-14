namespace Rpg
{
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using Rpg;
    using System.Collections;

    /// <summary>
    /// 
    /// </summary>
    public class HudManager : BaseManager<HudManager>
    {
        //Sliders
        public Slider healthSlider;
        public Slider karmaSlider;

        private static HudManager _instance;

        /// <summary>
        /// instance unique de la classe     
        /// </summary>
        public static HudManager instance
        {
            get
            {
                return _instance;
            }
        }

        protected override void Awake()
        {
            if (_instance != null)
            {
                throw new Exception("Tentative de création d'une autre instance de HudManager alors que c'est un singleton.");
            }
            _instance = this;

            if (Player.instance != null) Player.instance.OnAttack.AddListener(ChangeKarmaValue);
            if (Player.instance != null) Player.instance.OnDamaged.AddListener(ChangeLifeValue);
        }

        protected void Start()
        {

        }

        protected void Update()
        {

        }

        protected override IEnumerator CoroutineStart()
        {
            throw new NotImplementedException();
        }

        public void ChangeKarmaValue(float karmaValue)
        {
            karmaSlider.value = karmaValue;
        }

        public void ChangeLifeValue(float healthValue)
        {
            healthSlider.value = healthValue;
        }

        protected override void OnDestroy()
        {
            _instance = null;
        }
    }
}