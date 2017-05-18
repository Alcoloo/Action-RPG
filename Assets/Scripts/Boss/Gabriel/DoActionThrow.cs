using BehaviorDesigner.Runtime.Tasks;
using Rpg;
using Rpg.Characters;
using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class DoActionThrow : MonoBehaviour
    {
        public float _speed = 5.0f;
        public float _minDistance = 3.0f;
        public int _damage = 2;

        private Vector3 startPos;
        private Vector3 endPos;


        void Start()
        {
            endPos = Player.instance.transform.position + Vector3.up;
            startPos = transform.position;
        }

        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * _speed);

            if (Vector3.Distance(transform.position, Player.instance.transform.position) <= _minDistance)
            {
                Player.instance.GetComponent<Caracteristic>().TakeDamage(_damage, KIND.none);
                DestroyObject();
            }
            else if (Vector3.Distance(transform.position, endPos) <= _minDistance) DestroyObject();
        }

        private void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}