using Rpg.Manager;
using UnityEngine;


namespace Rpg.GraphicElement.Weapons
{

    /// <summary>
    /// 
    /// </summary>
    public class GunKind : Weapon
    {
        
        [SerializeField]
        private GameObject bulletTemplate;
        [SerializeField]
        private Transform transformToInstantiate;

        public float radiusDetect;
        public float viewAngle;

        protected override void Init()
        {
            base.Init();
            weaponController.curentShooter = this;

            /*GunCarac templateCarac = SaveManager.manager.GetGunCarac(_weaponAsset);   
            if (templateCarac == null) templateCarac = BallancingManager.manager.getGunCarac(_weaponAsset); 
            if(templateCarac != null)
            {
                _attack = templateCarac.damage;
                _align = templateCarac.align;
                hand = templateCarac.hand;
                bulletTemplate = templateCarac.shoot;
                transformToInstantiate = templateCarac.transformToInstantiate;
            }*/
        }
        public override void DoAction()
        {
            base.DoAction();
            GetNearEnnemy();
        }

        public void Shoot(Quaternion rotation,Vector3 position)
        {
            //if(EnemyManager.manager.ennemyNear.Count > 0)
            // {
           
                GameObject go = PoolingManager.manager.getFromPool(bulletTemplate.name);
                ThrowableKind currentShoot = go.GetComponent<ThrowableKind>();
                Player.instance.transform.LookAt(EnemyManager.manager.ennemyNear[0]);
                currentShoot.initialise(_weaponController, _unavailableTag,_attack,transform.position,transform.rotation);
           // }
        }

        private void GetNearEnnemy()
        {
            EnemyManager.manager.ennemyNear.Clear();
            Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, radiusDetect);

            for (int i = 0; i < targetInViewRadius.Length; i++)
            {
                Transform target = targetInViewRadius[i].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                if(Vector3.Angle(transform.forward,directionToTarget) < viewAngle / 2 && target.tag == "Ennemy")
                {
                    EnemyManager.manager.ennemyNear.Add(target);
                    Debug.Log(EnemyManager.manager.ennemyNear.Count);
                    //EnemyManager.manager.ennemyNear.Sort();
                }
            }
        }
    }
}