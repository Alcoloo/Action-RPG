using UnityEngine;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using HelpURL = BehaviorDesigner.Runtime.Tasks.HelpURLAttribute;
using BehaviorDesigner.Runtime.Formations.Tasks;
using BehaviorDesigner.Runtime;

namespace Rpg {

    /// <summary>
    /// 
    /// </summary>
    public class MoveToTarget : NavMeshFormationGroup {

        [Tooltip("The distance to stop moving")]
        public SharedFloat distance = 5;

        private List<Vector3> offsets = new List<Vector3>();

        protected override Vector3 TargetPosition(int index, float zLookAhead) {
            if (offsets.Count <= index) {
                return Vector3.zero;
            }

            var leaderTransform = leader.Value == null ? transform : leader.Value.transform;
            float agentOffset = 0;

            return leaderTransform.TransformPoint(offsets[index].x * (index % 2 == 0 ? -1 : 1) + agentOffset, 0, offsets[index].z + zLookAhead);
        }

        public override TaskStatus OnUpdate() {
            base.OnUpdate();

            if (Vector3.Distance(transform.position, targetTransform.Value.position) <= distance.Value) return TaskStatus.Success;
            else return TaskStatus.Running;
        }
    }
}