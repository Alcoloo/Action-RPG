using BehaviorDesigner.Runtime.Tasks;
using Rpg;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class DoActionRush : Action
    {
        public float chargeRush;
        private float startChargeRush;
        public float _minDistance;
        private int _rushNumber = 0;
        public int _rushMax = 6;
        public float speed;
        private Vector3 endPos;

        public override void OnStart()
        {
            startChargeRush = CustomTimer.manager.elapsedTime;
            endPos = Player.instance.transform.position + Vector3.up;
        }

        public override TaskStatus OnUpdate()
        {
            if (CustomTimer.manager.isTime(startChargeRush, chargeRush))
            {
                transform.LookAt(Player.instance.transform);
                Vector3 startPos = transform.position;
                
                transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * speed);

                if (Vector3.Distance(transform.position, endPos) < _minDistance && _rushNumber < _rushMax)
                {
                    return TaskStatus.Success;
                }
                else return TaskStatus.Running;
            }
            else return TaskStatus.Running;
        }
        
    }
}