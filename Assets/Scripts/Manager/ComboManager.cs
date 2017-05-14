using System;
using System.Collections;
using System.Collections.Generic;
using Rpg;
using UnityEngine;
using Rpg.Controller;

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
    public class ComboManager : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<string, Attack> listCombo;
        private string currentCombo;
        private int comboTimer = 60;
        private int currentComboTimer = 0;

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
            }
            currentComboTimer++;
        }

        public Attack Combo(string lInput)
        {
            currentCombo = currentCombo + lInput;
            currentComboTimer = 0;
            Debug.Log("CurrentCombo : " + currentCombo);
            if(listCombo.ContainsKey(currentCombo)) return listCombo[currentCombo];
            else
            {
                currentCombo = "";
                return listCombo[lInput];
            }

        }
    }
}
