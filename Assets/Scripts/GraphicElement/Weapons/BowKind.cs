using Rpg.Manager;
using UnityEngine;


namespace Rpg.GraphicElement.Weapons
{

    /// <summary>
    /// 
    /// </summary>
    public class BowKind : Weapon
    {
        
        [SerializeField]
        private GameObject _bulletTemplate;
        [SerializeField]
        private Transform transformToInstantiate;

        public float radiusDetect;
        public float viewAngle;

        protected override void Update()
        {
            base.Update();
            GetNearEnnemy();
        }

        public void Shoot(Quaternion rotation,Vector3 position)
        {
            // TODO lier la pool + position Bullet !
            GameObject go = Instantiate(_bulletTemplate,transformToInstantiate); //PoolingManager.manager.getFromPool("Shoot");
            ThrowableKind currentShoot = go.GetComponent<ThrowableKind>();
            //Player.instance.transform.LookAt(EnemyManager.manager.ennemyNear[0]);
            go.transform.position = position;
            go.transform.rotation = rotation;
            currentShoot.initialise(m_unavailableTag,m_damage,new Vector3(position.x,position.y +1f,position.z),rotation /*,EnemyManager.manager.ennemyNear[0].transform.position*/);
            Debug.Log("here");
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
                }
            }
        }
    }
}