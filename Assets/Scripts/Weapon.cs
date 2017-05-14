using Rpg.Controller;
using UnityEngine;
using UnityEngine.Events;

namespace Rpg
{
    public enum KIND { angelic, demonic , both, none };
    public enum WEAPONKIND { sword, gun };
    public enum HANDKIND { right, left, two_hand };
    public class WeaponCreationEvent : UnityEvent<Weapon>
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class Weapon : MonoBehaviour
    {
      
        [SerializeField]
        protected string _weaponAsset;
        [SerializeField]
        protected KIND _align;
        [SerializeField]
        protected HANDKIND hand;
        [SerializeField]
        protected WeaponController _weaponController;
        [SerializeField]
        protected string _unavailableTag;

        protected bool _isAttack = false;
        public string weaponAsset
        {
            get { return _weaponAsset; }
        }
        public KIND align
        {
            get { return _align; }
        }
        public WeaponController weaponController
        {
            get { return _weaponController; }
        }
        protected void Start()
        {
            Init();
        }
        protected void Update()
        {
            DoAction();
        }

        protected virtual void Init()
        {
            
        }
        public virtual void DoAction()
        {

        }
        public  void ActivateAttack(bool state)
        {
            _isAttack = state;
        }
        protected void AssignHand()
        {
            if(hand == HANDKIND.left)
            {
                weaponController.leftHand = this;
            }
            else if (hand == HANDKIND.right)
            {
               
                weaponController.rightHand = this;
            }
        }
        protected void RemoveHand()
        {
            if (hand == HANDKIND.left)
            {
                if (weaponController.leftHand == this)
                {
                    weaponController.leftHand = null;
                    return;
                }
            }
            else if (hand == HANDKIND.right)
            {
                if (weaponController.rightHand == this)
                {
                    weaponController.rightHand = null;
                    return;
                }
            }
        }


       


    }
}