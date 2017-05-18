using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Manager
{

    /// <summary>
    /// 
    /// </summary>
    /// 
    [System.Serializable]
    public class WeaponCarac
    {
        public string WeaponName;

        public int damage;
        public HANDKIND hand;
        public KIND align;
    }

    [System.Serializable]
    public class GunCarac : WeaponCarac
    {
        public GameObject shoot;
    }

    public class BaseCarac
    {
        public string name;

        public int Pv;
        public int MaxPv;
        public int armor;
    }

    [System.Serializable]
    public class PlayerCarac : BaseCarac
    {
        public int maxJumpSpeed;
        public int maxInAirSpeed;
        public int rollSpeed;
        public int walkSpeed;
        public int runSpeed;
    }

    [System.Serializable]
    public class BossCarac : BaseCarac
    {
       // mettez les caracteristique des boss ici
    }

    public class BallancingManager : BaseManager<BallancingManager>
    {
        [Header("Weapons")]
        [SerializeField]
        private List<WeaponCarac> swordList  = new List<WeaponCarac>();
        [SerializeField]
        private List<GunCarac> gunList = new List<GunCarac>();
        [Header("Player")]
        [SerializeField]
        private PlayerCarac _player = new PlayerCarac();
        [Header("Boss")]
        [SerializeField]
        private List<BossCarac> bossList = new List<BossCarac>();

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
        protected override IEnumerator CoroutineStart()
        {
            throw new NotImplementedException();
        }

        public WeaponCarac getSwordCarac(string pName)
        {
            foreach (WeaponCarac weapon in swordList)
            {
                if (weapon.WeaponName == pName) return weapon;
            }
            return null;
        }

        public GunCarac getGunCarac(string pName)
        {
            foreach (GunCarac weapon in gunList)
            {
                if (weapon.WeaponName == pName) return weapon;
            }
            return null;
        }
        public BossCarac getBossCarac(string pName)
        {
            foreach (BossCarac boss in bossList)
            {
                if (boss.name == pName) return boss;
            }
            return null;
        }


    }
}