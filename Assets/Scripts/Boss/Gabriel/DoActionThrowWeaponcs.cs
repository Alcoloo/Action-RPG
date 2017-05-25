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
        public float speed;
        public float _minDistance;
        public int _damage;
        public float _lerpSpeed;

        private Vector3 startPos;
        private Vector3 endPos;
        private bool isHit;
        private Quaternion playerRot;
        private Quaternion goRot;


        public override void OnStart()
        {
            startPos = weapon.transform.position;
            endPos = Player.instance.transform.position + Vector3.up;
            isHit = false;
        }

        public override TaskStatus OnUpdate()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, playerRot, _lerpSpeed * Time.deltaTime);
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
                    Player.instance.GetComponent<Caracteristic>().TakeDamage(_damage, KIND.none);
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
            endPos = Player.instance.transform.position;
            playerRot = Quaternion.LookRotation(Player.instance.transform.position);
            goRot = transform.rotation;
        }
        
           
    }
}