using Rpg.Controller;
using UnityEngine;

namespace Rpg.GraphicElement.Weapons
{

    /// <summary>
    /// 
    /// </summary>
    public class ThrowableKind : SwordKind
    {
        [SerializeField]
        private float _bulletSpeed = 0.5F;

        protected override void Init()
        {
            weaponController.shoots.Add(this);
        }
        override protected void OnCollisionEnter(Collision col)
        {
            base.OnCollisionEnter(col);
            Destroy(gameObject);
        }
        public override void OnDisable()
        {
           
        }
        public override void OnEnable()
        {
            Debug.Log(gameObject.name);
        }
        public override void DoAction()
        {
            base.DoAction();
            transform.position += transform.forward * _bulletSpeed;
        }
        public void initialise(WeaponController wc, string tag)
        {
            _weaponController = wc;
            _unavailableTag = tag;
            _isAttack = true;
        }
       
        public void OnDestroy()
        {
            weaponController.shoots.Remove(this);
        }
        

    }
}