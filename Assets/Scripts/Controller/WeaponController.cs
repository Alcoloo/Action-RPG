using Rpg.GraphicElement.Weapons;
using Rpg.Manager;
using System.Collections.Generic;
using UnityEngine;


namespace Rpg.Controller
{

    /// <summary>
    /// 
    /// </summary>
    public class WeaponController : BaseController
    {
        
      
        [SerializeField]
        public BowKind curentShooter;
        [SerializeField]
        public List<SwordKind> swords;
        [SerializeField]
        public List<ThrowableKind> shoots;

        public Weapon leftHand;
        public Weapon rightHand;

        


        protected override void AwakeController()
        {
            base.AwakeController();

        }
        protected override void InitController()
        {
            base.InitController();
        }

        protected override void DoAcTion()
        {
            base.DoAcTion();
        }

        public void activateWeaponAttack(HAND hand,bool state)
        {
            if(hand == HAND.left)
            {
                leftHand.ActivateAttack(state);
            }
            else if(hand == HAND.right)
            {
                rightHand.ActivateAttack(state);
            }
            else
            {
                leftHand.ActivateAttack(state);
                rightHand.ActivateAttack(state);
            }
        }
    }
}