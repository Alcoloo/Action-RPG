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

        public void DoAction()
        {
            GetNearEnnemy();
        }

        public void Shoot(Quaternion rotation,Vector3 position)
        {
            // TODO lier la pool + position Bullet !
            GameObject go = Instantiate(_bulletTemplate);
            ThrowableKind currentShoot = go.GetComponent<ThrowableKind>();
            Player.instance.transform.LookAt(EnemyManager.manager.ennemyNear[0]);
            currentShoot.initialise(m_unavailableTag,m_damage,transform.position,transform.rotation, EnemyManager.manager.ennemyNear[0].transform.position);
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