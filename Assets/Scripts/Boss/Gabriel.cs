using UnityEngine;
using System;
using System.Collections.Generic;
using Rpg;
using System.Collections;

namespace Assets.Scripts.Boss
{

    /// <summary>
    /// 
    /// </summary>
    public class Gabrielle : MonoBehaviour
    {
        delegate void Pattern();
        private List<Pattern> tabPattern = new List<Pattern>();

        private float charge = 0.0f;
        private bool startCharging = false;

        public GameObject weapon;
        public GameObject tornado;

        private static Gabrielle _instance;
        

        /// <summary>
        /// instance unique de la classe     
        /// </summary>
        public static Gabrielle instance
        {
            get
            {
                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance != null)
            {
                throw new Exception("Tentative de création d'une autre instance de Gabriel alors que c'est un singleton.");
            }
            _instance = this;
        }

        private void Start()
        {

        }

        private void Update()
        {
            
        }

        private void DemonicCharge()
        {
            ReleasePower();
            ChargeToPlayer();  
        }

        

        private bool ChargePower(int chargeTime)
        {
            charge += Time.deltaTime;
            Debug.Log("charge : " + charge);
            if (charge >= chargeTime) return true;
            else return false;
        }

        private void DoActionTornado()
        {
            Instantiate(tornado, transform);
            tornado.transform.Translate(transform.position);
        }

        private void ReleasePower()
        {
            //Ici il y aura un acces au Player pour le passer en Mode Void un certain temps
        }

        private void ChargeToPlayer()
        {
            transform.position = Vector3.Lerp(transform.position,Player.instance.transform.position,25000.0f);
        }

        protected void OnDestroy()
        {
            _instance = null;
        }
    }
}