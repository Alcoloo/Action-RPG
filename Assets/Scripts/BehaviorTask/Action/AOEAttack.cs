using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Rpg {

    /// <summary>
    /// 
    /// </summary>
    public class AOEAttack : Action {

        [BehaviorDesigner.Runtime.Tasks.Tooltip("GameObject use to show Area Attack")]
        public SharedGameObject area = null;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("GameObject use to potisionate Area Attack")]
        public SharedGameObject target = null;

        public override void OnStart() {
            MonoBehaviour.Instantiate(area.Value, target.Value.transform.position, Quaternion.Euler(Vector3.zero));
        }

        public override TaskStatus OnUpdate() {
            return TaskStatus.Success;
        }
    }
}