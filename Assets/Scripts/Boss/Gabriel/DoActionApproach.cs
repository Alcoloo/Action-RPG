
using BehaviorDesigner.Runtime.Tasks;
using Rpg;
using Rpg.Characters;
using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{

    /// <summary>
    /// 
    /// </summary>
    public class DoActionApproach : Action
    {

        public float _speed;
        public float _minDistance;
        public int _hitFactor;
        public int _maxHitPattern;
        public float _patternTime;
        public float attackTime;

        private Caracteristic cara;
        private int hitCount;
        private float startPatternTime;

        public override void OnStart()
        {
            cara = GetComponent<Caracteristic>();
            cara.isHit.AddListener(UpgradeHitCount);
            startPatternTime = CustomTimer.manager.elapsedTime;
        }

        public override TaskStatus OnUpdate()
        {
            transform.LookAt(Player.instance.transform);
            if (Vector3.Distance(transform.position, Player.instance.transform.position) <= _minDistance)
            {
                if (hitCount % _hitFactor == 0 && hitCount != 0) DoBasicAttack();
                else if (hitCount >= _maxHitPattern) return TaskStatus.Success;
                if (CustomTimer.manager.isTime(startPatternTime, attackTime)) DoBasicAttack();
            }
            else if (CustomTimer.manager.isTime(startPatternTime, _patternTime)) return TaskStatus.Success;
            else transform.position = Vector3.MoveTowards(transform.position, Player.instance.transform.position + Vector3.up, Time.deltaTime * _speed);
            return TaskStatus.Running;
        }

        private void UpgradeHitCount(int pv, int _pv2)
        {
            hitCount++;
        }

        private void DoBasicAttack()
        {
            Debug.Log("Attack");
        }
        
    }
}