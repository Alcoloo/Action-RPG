﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Rpg.Characters
{

    /// <summary>
    /// 
    /// </summary>
    
    public class HealthEvent : UnityEvent<int,int>
    {
        void isHit(int _pv, int _maxpv)
        {
            
        }
    }
    public enum TOUCHABLESTATE { god, normal }
    public class Caracteristic : MonoBehaviour
    {
        [SerializeField]
        private int _pv =10;
        [SerializeField]
        private int _maxPv = 10;
        [SerializeField]
        private int _armor = 2;
        [SerializeField]
        private ALIGN align = ALIGN.both;
        [SerializeField]
        private TOUCHABLESTATE state = TOUCHABLESTATE.normal;

        public UnityEvent isDeath;
        public HealthEvent isHit;
        public HealthEvent isHeal;

        public int pv
        {
            get { return _pv; }
        }

        protected void Awake()
        {
            isDeath = new UnityEvent();
            isHit = new HealthEvent();
            isHeal = new HealthEvent();
        }
        protected void Start()
        {
           // StartCoroutine(CotoDamage());
        }

        protected void Update()
        {
           
        }
    
        private void getHit (int damage)
        {
            if(damage <= 0)
            {
                return;
            }
            _pv -= damage;
            if(_pv <= 0)
            {
                Debug.Log(name+" : Death");
                isDeath.Invoke();
                return;
            }
            isHit.Invoke(_pv,_maxPv);
        } 

        public void Heal(int healPoint)
        {
            _pv += healPoint;
            if(_pv > _maxPv)
            {
                _pv = _maxPv;
            }
            isHeal.Invoke(_pv,_maxPv);
        }

        public void UpdateMaxPV(int maxPv)
        {
            _maxPv = maxPv;
        }

        public void TakeDamage(int pAttackPoint, ALIGN pAlign)
        {
            if (state == TOUCHABLESTATE.god) return;
            if (_pv <= 0) return;

            switch (align)
            {
                case ALIGN.both:
                    break;
                case ALIGN.angelic:
                    if (pAlign != ALIGN.angelic) return;
                    break;
                case ALIGN.demonic:
                    if (pAlign != ALIGN.demonic) return;
                    break;
                case ALIGN.none:
                    return;    
            }
             
            int damage;
            damage = pAttackPoint - _armor;
            if (damage < 0) damage = 0;
            getHit(damage);
        }
        private void CalculateCarac()
        {
            /// mettre une potentielle formule de calcul des caracteristiques si on part sur d'autre type de carcterstiques ^^  
        }

        public void setAlign(ALIGN pAlign)
        {
            align = pAlign;
        }
        public void setTouchable(TOUCHABLESTATE pState)
        {
            state = pState;
        }
        public void setCarac(int pv,int maxpv, int armor, ALIGN align = ALIGN.both, TOUCHABLESTATE state = TOUCHABLESTATE.normal)
        {
            _pv = pv;
            _maxPv = maxpv;
            _armor = armor;
            setAlign(align);

        }
    }
}