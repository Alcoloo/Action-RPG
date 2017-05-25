namespace Rpg
{
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using Rpg;
    using System.Collections;
    using Assets.Scripts.Boss.Gabriel;
    using Characters;
    using Manager;

    /// <summary>
    /// 
    /// </summary>
    public class HudManager : BaseManager<HudManager>
    {
        //Sliders
        public Slider healthSlider;
        public Slider karmaSlider;
        public Slider bossSlider;
        public Text comboText;
        public Text moneyNumberTxt;
        public GameObject HudPanel;

        private int _moneyFactor;
        

        /// <summary>
        /// instance unique de la classe     
        /// </summary>

        protected override void Awake()
        {
            base.Awake();

        }

        protected void Start()
        {
            if (Player.instance != null) Player.instance.OnAttack.AddListener(ChangeKarmaValue);
            if (Player.instance != null) Player.instance.OnDamaged.AddListener(ChangeLifeValue);

            if (Gabriel.instance != null) Gabriel.instance.GetComponent<Caracteristic>().isHit.AddListener(ChangeBossLifeValue);
            if (ComboManager.manager != null) ComboManager.manager.weaponHit.AddListener(IncreaseComboNumber);
        }

        protected void Update()
        {

        }

        public void OnComboFinish(int comboNumber)
        {
            int moneyNumber = comboNumber * _moneyFactor;
            moneyNumberTxt.text = moneyNumber.ToString();
        }

        private void IncreaseComboNumber()
        {
            int comboNumber = ComboManager.manager.GetComboNumber();
            comboText.text = comboNumber.ToString();
        }

        private void ChangeBossLifeValue(int pv, int _maxPv)
        {
            bossSlider.value = pv;
        }
        

        private void ChangeKarmaValue(float karmaValue)
        {
            karmaSlider.value = karmaValue;
        }

        private void ChangeLifeValue(float healthValue)
        {
            healthSlider.value = healthValue;
        }

        public void ChangeHUDVisibility()
        {
            Debug.Log(HudPanel.activeSelf);
            HudPanel.SetActive(!HudPanel.activeSelf);
        }
        
    }
}