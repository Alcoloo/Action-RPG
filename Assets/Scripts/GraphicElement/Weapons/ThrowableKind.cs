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
        private bool canMove = false;
        private int _damage = 10;

        protected void OnCollisionEnter(Collision col)
        {
            if (col.collider.tag == "Enemy")
            {
                col.gameObject.GetComponent<Enemy>().TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
        
        public void Update()
        {
            if (canMove)
            {
                if (EnemyManager.manager.ennemyNear.Count > 0) transform.position = Vector3.MoveTowards(transform.position, EnemyManager.manager.ennemyNear[0].transform.position, _bulletSpeed);
                else transform.position += transform.forward * _bulletSpeed;
            }
        }

        public void initialise(string tag, int damage, Vector3 position,Quaternion rotation)// Vector3 pTarget
        {
            gameObject.SetActive(false);
            transform.position = position;
            transform.rotation = rotation;
            _startTime = CustomTimer.manager.elapsedTime;
            canMove = true;
            Debug.Log("ici");
            gameObject.SetActive(true);
        }
       
        public void OnDestroy()
        {
           
        }
    }
}