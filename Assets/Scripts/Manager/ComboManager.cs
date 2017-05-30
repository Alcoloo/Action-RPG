using System;
 
using System.Collections;
 
using System.Collections.Generic;
 
using Rpg;
 
using UnityEngine;
 
using Rpg.Controller;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Rpg.Manager
{
    public class Attack
    {
        public int side;
        public int number;
    }
    
    public class OnComboFinishEvent : UnityEvent<int> { }
    public class OnComboUpgradeEvent : UnityEvent<int> { }

    /// <summary>
    /// 
    /// </summary>
    public class ComboManager : BaseManager<ComboManager>
    {

        public BaseGameEvent weaponHit;
        public OnComboUpgradeEvent comboUpgrade;
        public OnComboFinishEvent onComboFinish;
        [SerializeField]
        private static Dictionary<string, Attack> _listCombo;
        private string _currentCombo = "";
        private int _delayCancelChain = 3;
        private int _delayWaitInput = 1;
        private int _currentComboTimer = 0;
        public float _startComboAnim;
        public float _startWaitInput;
        [SerializeField]
        private int _delayReset = 5;
        private float _startComboTimer = 0;
        private int _currentComboNumber = 0;
        private int _totalComboNumber = 0;

        protected override void Awake() 
        {
            manager = this;
        }
 
        void Start() 
        {
            Init();
        }
 
        private void Update() 
        { 
            UpdateCombo(); 
        }
 
        public void Init() 
        { 
            weaponHit = new BaseGameEvent(); 
            comboUpgrade = new OnComboUpgradeEvent(); 
            onComboFinish = new OnComboFinishEvent(); 
            initCombo(); 
        }
 
        public void initCombo() 
        { 
            _listCombo = new Dictionary<string, Attack>(); 
            //COMBO EPEE 
            _listCombo.Add("X", new Attack() {side=2,number=8}); 
            _listCombo.Add("XX", new Attack() { side = 1, number = 1 }); 
            _listCombo.Add("XXX", new Attack() { side = 1, number = 4 }); 
            _listCombo.Add("X-X", new Attack() { side = 3, number = 1 }); 
            _listCombo.Add("X-XX", new Attack() { side = 2, number = 9 }); 
            //COMBO ARC
 
            _listCombo.Add("B", new Attack() { side = 3, number = 1 }); 
            _listCombo.Add("BB", new Attack() { side = 3, number = 5 }); 
            _listCombo.Add("BBB", new Attack() { side = 3, number = 4 }); 
        }
 
        public void UpdateCombo() 
        { 
            // Delai Combo Amount 
            if (_startComboTimer != 0) 
            { 
                if (CustomTimer.manager.isTime(_startComboTimer, _delayReset)) 
                { 
                    _totalComboNumber += _currentComboNumber; 
                    onComboFinish.Invoke(_totalComboNumber); 
                    _currentComboNumber = 0; 
                    _startComboTimer = 0; 
                } 
            } 
            // Delai Combo Anim 
            if (_startComboAnim != 0) 
            { 
                if (CustomTimer.manager.isTime(_startComboAnim, _delayCancelChain)) 
                { 
                    _currentCombo = ""; 
                    _currentComboTimer = 0; 
                    _startComboAnim = 0; 
                } 
            }
             
            if (_startWaitInput != 0) 
            { 
                if (CustomTimer.manager.isTime(_startComboAnim, _delayWaitInput)) 
                { 
                    _currentCombo = _currentCombo + "-"; 
                    _startWaitInput = 0; 
                } 
            } 
        }
         
        public Attack Combo(string lInput) 
        { 
            _currentCombo = _currentCombo + lInput; 
            _currentComboTimer = 0; 
            _startComboAnim = CustomTimer.manager.elapsedTime; 
            _startWaitInput = CustomTimer.manager.elapsedTime; 
            if(_listCombo.ContainsKey(_currentCombo)) return _listCombo[_currentCombo]; 
            else 
            { 
                _currentCombo = "";
                 return _listCombo[lInput]; 
            } 
        }
         
        public void IncreaseCombo() 
        {
             _startComboTimer = CustomTimer.manager.elapsedTime; 
            _currentComboNumber++; 
            weaponHit.Invoke(); 
            comboUpgrade.Invoke(_currentComboNumber); 
        }
 
        void OnEnable() 
        { 
            SceneManager.sceneLoaded += OnSceneLoaded; 
        }
 
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) 
        {            
            Init(); 
        }
 
        void OnDisable() 
        { 
        }
 
    }
 
}
 
