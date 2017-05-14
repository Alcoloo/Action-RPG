using BehaviorDesigner.Runtime.Tasks;
using Rpg;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class DoActionThrowWeaponcs : Action
    {

        delegate void LaunchAction();
        private LaunchAction DoAction;

        public GameObject weapon;

        private Vector3 startPos;
        private Vector3 endPos;
        public float speed;
        private bool isHit = false;

        protected void Start()
        {
            startPos = weapon.transform.position;
            SetModeReplace();
        }

        protected void Update()
        {
            if(!isHit)DoAction();
        }

        private void SetModeThrow()
        {
            
            DoAction = DoActionThrow;
        }

        private void SetModeReplace()
        {
            Debug.Log("replace");
            DoAction = DoActionReplace;
        }

        private void DoActionThrow()
        {
            Debug.Log("throw");
            endPos = Player.instance.transform.position;
            Debug.Log(weapon.transform.position);
            weapon.transform.position = Vector3.MoveTowards(weapon.transform.position, endPos, Time.deltaTime * speed);

            if (Vector3.Distance(weapon.transform.position, endPos) <= 0.1f) SetModeReplace();
        }

        private void DoActionReplace()
        {
            weapon.transform.position = startPos;
            SetModeThrow();
        }
        

        

        
    }
}