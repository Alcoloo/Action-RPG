using UnityEngine;
using Rpg;
using Rpg.Controller;
using Rpg.Manager;

namespace Rpg
{

    /// <summary>
    /// 
    /// </summary>
    public class Karma : MonoBehaviour
    {
        private float startingKarma = 50;
        public float currentKarma;

        private WeaponController wp;

        void Awake()
        {
            
        }

        protected void Start()
        {
            currentKarma = startingKarma;
            if (ComboManager.manager != null) ComboManager.manager.weaponHit.AddListener(ChangeKarmaValue);
        }

        protected void Update()
        {

        }

        public void ChangeKarmaValue()
        {
            if (Player.instance.currentWeapon == WEAPON.SWORDS) currentKarma--;
            else currentKarma++;
            Player.instance.ChangeKarmaValue(currentKarma);
        }
    }
}