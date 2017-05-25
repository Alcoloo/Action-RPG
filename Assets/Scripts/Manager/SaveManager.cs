using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Manager
{

    /// <summary>
    /// 
    /// </summary>
    public class SaveManager : BaseManager<SaveManager>
    {


        private PlayerCarac _player;
        private Dictionary<string,WeaponCarac> _swordList = new Dictionary<string,WeaponCarac> ();
        private Dictionary<string,GunCarac> _gunList = new Dictionary<string, GunCarac>();

        public PlayerCarac player
        {
            get { return _player; }
        }

        protected void Start()
        {

        }

        protected void Update()
        {

        }
       
        #region Save functions


        public void SavePlayer(PlayerCarac pPlayer)
        {
            _player = pPlayer;
        }
        public void SaveSword(string pWeaponName,WeaponCarac pSword)
        {
            _swordList.Add(pWeaponName, pSword);
        }
        public void SaveGun(string pWeaponName, GunCarac pGun)
        {
            _gunList.Add(pWeaponName, pGun);
        }

        #endregion


        #region Restore function

        public WeaponCarac GetSwordCarac(string pWeaponName)
        {
            if (!_swordList.ContainsKey(pWeaponName)) return null;
            return _swordList[pWeaponName];
        }
        public GunCarac GetGunCarac(string pWeaponName)
        {
            if (!_gunList.ContainsKey(pWeaponName)) return null;
            return _gunList[pWeaponName];
        }
        #endregion


    }
}