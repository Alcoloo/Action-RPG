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

        private float _startTime;
        private bool canMove;

        protected override void Init()
        {
            weaponController.shoots.Add(this);
        }
        override protected void OnCollisionEnter(Collision col)
        {
            base.OnCollisionEnter(col);
            if (col.collider.tag == "Enemy") Destroy(gameObject);
        }
        public override void OnDisable()
        {
           
        }
        public override void OnEnable()
        {
           
        }

        public override void DoAction()
        {
            base.DoAction();
            if (EnemyManager.manager.ennemyNear.Count > 0) transform.position = Vector3.MoveTowards(transform.position, EnemyManager.manager.ennemyNear[currentEnnemyAimedIndex].transform.position, _bulletSpeed);
            else transform.position = Vector3.forward * _bulletSpeed;
        }
        public void initialise(WeaponController wc, string tag, int damage, Vector3 position,Quaternion rotation)
        {
            gameObject.SetActive(false);
            _weaponController = wc;
            _unavailableTag = tag;
            _isAttack = true;
            _attack = damage;
            _startTime = CustomTimer.manager.elapsedTime;
            canMove = true;
            gameObject.SetActive(true);
        }
       
        public void OnDestroy()
        {
           
        }
        

    }
}