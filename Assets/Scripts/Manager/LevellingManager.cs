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
        private float _comboMultiplierUnit = 0.1f;
        [SerializeField]
        private float _maxComboMultiplier = 2.0f;

        private float _comboMultiplier = 1.0f;

        protected void Start()
        {
            StartCoroutine(expCorroutine());
        }

        protected void Update()
        {

        }
        protected IEnumerator expCorroutine()
        {
            _levelController.gainExp(10);
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(expCorroutine());
        }
        protected override IEnumerator CoroutineStart()
        {
            throw new NotImplementedException();

        }
        public void resetComboMultiplier()
        {
            _comboMultiplier = 1.0f;
        }
        public void setComboMultiplier()
        {
            _comboMultiplier += _comboMultiplierUnit;
            if (_comboMultiplier > _maxComboMultiplier) _comboMultiplier = _maxComboMultiplier;
        }
        public void gainExp(int expToEarn)
        {
            Debug.Log("here");
            _levelController.gainExp((int)(expToEarn * _comboMultiplier));
        }
    }
}