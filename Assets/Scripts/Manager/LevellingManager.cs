using System;
using System.Collections;
using Rpg;
using UnityEngine;
using Rpg.Controller;

namespace Rpg.Manager
{

    /// <summary>
    /// 
    /// </summary>
    public class LevellingManager : BaseManager<LevellingManager>
    {
        [SerializeField]
        private LevelController _levelController;
        [SerializeField]
        private WeaponController _weaponController;

        [SerializeField]
        private float _comboMultiplierUnit = 0.1f;
        [SerializeField]
        private float _maxComboMultiplier = 2.0f;

        private float _comboMultiplier = 1.0f;

        protected void Start()
        {
            //StartCoroutine(expCorroutine());
            if(ComboManager.manager != null) ComboManager.manager.weaponHit.AddListener(AddComboMultiplier);
        }

        protected void Update()
        {

        }
        protected IEnumerator expCorroutine()
        {
            _levelController.GainExp(10);
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(expCorroutine());
        }
        public void resetComboMultiplier()
        {
            _comboMultiplier = 1.0f;
        }
        public void AddComboMultiplier()
        {
            _comboMultiplier += _comboMultiplierUnit;
            if (_comboMultiplier > _maxComboMultiplier) _comboMultiplier = _maxComboMultiplier;
            Debug.Log(_comboMultiplier);
        }
        public void GainExp(int expToEarn)
        {
            Debug.Log("here");
            _levelController.GainExp((int)(expToEarn * _comboMultiplier));
            resetComboMultiplier();
        }
    }
}