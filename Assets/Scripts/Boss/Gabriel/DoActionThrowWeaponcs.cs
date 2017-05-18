using BehaviorDesigner.Runtime.Tasks;
using Rpg;
using Rpg.Characters;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class DoActionThrowWeaponcs : Action
    {
        
        public GameObject weapon;

        private Vector3 startPos;
        private Vector3 endPos;
        public float speed;
        public float _minDistance;
        public int _damage;
        private bool isHit;


        public override void OnStart()
        {
            startPos = weapon.transform.position;
            endPos = Player.instance.transform.position + Vector3.up;
            isHit = false;
        }

        public override TaskStatus OnUpdate()
        {
            if (Gabriel.instance.HasBeenHit())
            {
                DoActionReplace();
                isHit = true;
            }
            if (!isHit)
            {
                weapon.transform.position = Vector3.MoveTowards(weapon.transform.position, endPos, Time.deltaTime * speed);

                if (Vector3.Distance(weapon.transform.position, Player.instance.transform.position) <= _minDistance)
                {
                    Player.instance.GetComponent<Caracteristic>().TakeDamage(_damage,KIND.none);
                    DoActionReplace();
                }
                else if (Vector3.Distance(weapon.transform.position, endPos) <= _minDistance) DoActionReplace();
                return TaskStatus.Running;
            }
            else return TaskStatus.Success;
        }

        private void DoActionReplace()
        {
            weapon.transform.position = startPos;
            transform.LookAt(Player.instance.transform);
            endPos = Player.instance.transform.position;
        }
        

        

        
    }
}