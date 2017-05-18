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
        }
        public override void DoAction()
        {
            base.DoAction();
            GetNearEnnemy();
        }

        public void Shoot(Quaternion rotation,Vector3 position)
        {
            if(EnemyManager.instance.ennemyNear.Count > 0)
            {
                GameObject go = Instantiate(bulletTemplate, transformToInstantiate);
                ThrowableKind currentShoot = go.GetComponent<ThrowableKind>();
                transform.LookAt(EnemyManager.instance.ennemyNear[0]);
                currentShoot.initialise(_weaponController, _unavailableTag);
                go.transform.position = new Vector3(position.x, position.y + 1.25f, position.z);
                go.transform.rotation = rotation;
            }
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
                    EnemyManager.instance.ennemyNear.Add(target);
                    Debug.Log(EnemyManager.instance.ennemyNear.Count);
                    //EnemyManager.instance.ennemyNear.Sort();
                }
            }
        }
    }
}