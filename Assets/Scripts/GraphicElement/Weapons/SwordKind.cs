using Rpg.Characters;
using UnityEngine;

namespace Rpg.GraphicElement.Weapons
{

    /// <summary>
    /// 
    /// </summary>
    
    public class SwordKind : Weapon
    {
       
        [SerializeField]
        private int _attack = 10;
       

        protected override void Init()
        {
            base.Init();
            weaponController.swords.Add(this);
        }
       
        public override void DoAction()
        {
            base.DoAction();
        
        }
        protected virtual void OnCollisionEnter(Collision col)
        {

            /*if (!_isAttack) return;

            if (col.collider.CompareTag(_unavailableTag)) return;
            Caracteristic carac = col.collider.GetComponent<Caracteristic>();
            if (carac == null) return;
            carac.TakeDamage(_attack,_align);
            */
            if (!_isAttack)
                return;
            else
            {
                Debug.Log("Is Attacking");
                if (col.collider.CompareTag("Ennemy"))
                {
                    Debug.Log("Target is an enemy");
                    Caracteristic carac = col.collider.GetComponent<Caracteristic>();
                    carac.TakeDamage(_attack, _align);
                }
            }
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