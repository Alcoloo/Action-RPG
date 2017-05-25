using Rpg.Characters;
using Rpg.Manager;
using UnityEngine;

namespace Rpg.GraphicElement.Weapons
{

    /// <summary>
    /// 
    /// </summary>
    
    public class SwordKind : Weapon
    {
       
      
       

        protected override void Init()
        {
            base.Init();
            weaponController.swords.Add(this);
            /*WeaponCarac templateCarac = SaveManager.manager.GetSwordCarac(_weaponAsset);
            if (templateCarac == null) templateCarac = BallancingManager.manager.getSwordCarac(_weaponAsset);
            if (templateCarac != null)
            {
                _attack = templateCarac.damage;
                _align = templateCarac.align;
                hand = templateCarac.hand;
            }*/
        }
       
        public override void DoAction()
        {
            base.DoAction();
        
        }
        protected virtual void OnCollisionEnter(Collision col)
        {
           if (!_isAttack) return;
            
           if (col.collider.CompareTag(_unavailableTag)) return;
           Caracteristic carac = col.collider.GetComponent<Caracteristic>();
           if (carac == null) return;
           carac.TakeDamage(_attack,_align);
           ComboManager.manager.IncreaseCombo();
            
        }
        public virtual void OnEnable()
        {
            AssignHand();
        }
        public  virtual void OnDisable()
        {
            RemoveHand();
        }

    }
}