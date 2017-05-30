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
    using UnityEngine.SceneManagement;

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

            if (ComboManager.manager != null) ComboManager.manager.comboUpgrade.AddListener(IncreaseComboNumber);
            if (ComboManager.manager != null) ComboManager.manager.onComboFinish.AddListener(OnComboFinish);
        }

        protected void Update()
        {

        }

        public void ActiveSliderBoss()
        {
            bossSlider.gameObject.SetActive(true);
        }

        public void OnComboFinish(int comboNumber)
        {
            int moneyNumber = comboNumber;
            moneyNumberTxt.text = moneyNumber.ToString();
            comboText.text = "0";
        }

        private void IncreaseComboNumber(int comboNumber)
        {
            comboText.text = comboNumber.ToString();
        }

        public void ChangeBossLifeValue(float pv)
        {
            bossSlider.value = pv;
        }
        

        private void ChangeKarmaValue(float karmaValue)
        {
            karmaSlider.value = karmaValue;
            Debug.Log(karmaSlider.value);
        }

        private void ChangeLifeValue(float healthValue)
        {
            healthSlider.value = healthValue;
        }

        public void ChangeHUDVisibility()
        {
            HudPanel.SetActive(!HudPanel.activeSelf);
        }
        
    }
}