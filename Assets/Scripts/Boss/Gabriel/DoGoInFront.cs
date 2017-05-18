using BehaviorDesigner.Runtime.Tasks;
using Rpg;
using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class DoGoInFront : Action
    {
        public float _speed;
        public float _movingTime;
        public GameObject weapon;
        private GameObject weaponGO;

        private Transform playerTransform;
        private float startMovingTime;

        public override void OnStart()
        {
            playerTransform = Player.instance.transform;
            startMovingTime = CustomTimer.instance.elapsedTime;
        }

        public override TaskStatus OnUpdate()
        {
            transform.LookAt(playerTransform);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,transform.position.y,playerTransform.position.z), Time.deltaTime * _speed);
            if (transform.position.z == playerTransform.position.z)
            {
                weaponGO = GameObject.Instantiate(weapon, weapon.transform,true);
                weaponGO.AddComponent<DoActionThrow>();
                return TaskStatus.Success;
            }
            else if (CustomTimer.instance.isTime(startMovingTime, _movingTime)) return TaskStatus.Success;
            else return TaskStatus.Running;
        }
    }
}