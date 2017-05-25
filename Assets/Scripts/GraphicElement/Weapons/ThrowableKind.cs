using Rpg.Controller;
using UnityEngine;

namespace Rpg.GraphicElement.Weapons
{

    /// <summary>
    /// 
    /// </summary>
    public class ThrowableKind : MonoBehaviour
    {
        [SerializeField]
        private float _bulletSpeed = 2F;
        private GameObject _father;
        private float _timeOut = 5f;
        private float _startTime;

        protected void OnCollisionEnter(Collision col)
        {
            if (col.collider.tag == "Enemy") Destroy(gameObject);
        }
        
        public void DoAction()
        {
            if(CustomTimer.manager.isTime(_startTime,_timeOut)) Destroy(gameObject);
            else
            {
                if (EnemyManager.manager.ennemyNear.Count > 0) transform.position = Vector3.MoveTowards(transform.position, EnemyManager.manager.ennemyNear[0].transform.position, _bulletSpeed);
                else transform.position = Vector3.forward * _bulletSpeed;
            }
        }

        public void initialise(string tag, int damage, Vector3 position,Quaternion rotation, Vector3 pTarget)
        {
            gameObject.SetActive(false);
            _startTime = CustomTimer.manager.elapsedTime;
            gameObject.SetActive(true);
        }
       
        public void OnDestroy()
        {
           
        }
        

    }
}