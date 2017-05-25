using System;
using System.Collections;
using System.Collections.Generic;
using Rpg;
using UnityEngine;
using Rpg.Controller;
using UnityEngine.Events;

namespace Rpg.Manager
{
    public class Attack
    {
        public int side;
        public int number;
    }

    /// <summary>
    /// 
    /// </summary>
    public class ComboManager : BaseManager<ComboManager>
    {

        public BaseGameEvent weaponHit;
        [SerializeField]
        private Dictionary<string, Attack> listCombo;
        private string currentCombo;
        private int comboTimer = 60;
        private int currentComboTimer = 0;
        private int currentComboNumber;

        private static ComboManager m_Manager;

        protected override void Awake()
        {
            m_Manager = this;
            manager = this;
            weaponHit = new BaseGameEvent();
            initCombo();
        }
        
        private void Update()
        {
            UpdateCombo();
        }

        public void initCombo()
        {
            listCombo = new Dictionary<string, Attack>();
            //COMBO EPEE
            listCombo.Add("X", new Attack() {side=2,number=8});
            listCombo.Add("XX", new Attack() { side = 1, number = 1 });
            listCombo.Add("XXX", new Attack() { side = 1, number = 4 });
            //COMBO ARC
            listCombo.Add("B", new Attack() { side = 3, number = 1 });
            listCombo.Add("BB", new Attack() { side = 3, number = 5 });
            listCombo.Add("BBB", new Attack() { side = 3, number = 4 });
        }

        public void UpdateCombo()
        {
            if (currentComboTimer > comboTimer)
            {
                currentCombo = "";
                ComboFinish();
                currentComboNumber = 0;
            }
            currentComboTimer++;
        }

        public Attack Combo(string lInput)
        {
            currentCombo = currentCombo + lInput;
            currentComboTimer = 0;
            if(listCombo.ContainsKey(currentCombo)) return listCombo[currentCombo];
            else
            {
                currentCombo = "";
                return listCombo[lInput];
            }
        }

        private void ComboFinish()
        {
            //HudManager.manager.OnComboFinish(currentComboNumber);
        }

        public void IncreaseCombo()
        {
            weaponHit.Invoke();
            currentComboNumber++;
        }

        public int GetComboNumber()
        {
            return currentComboNumber;
        }
    }
}
