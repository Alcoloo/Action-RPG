using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Rpg.Controller;
using Rpg;

public class AttackSword : Action {

    [BehaviorDesigner.Runtime.Tasks.Tooltip("GameObject who attack.")]
    public SharedGameObject target = null;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("GameObject use for attack.")]
    public SharedGameObject weapon = null;
    
    private bool horizontal = false;
    private bool isComplete = false;

    public override void OnStart() {
        
        horizontal = Random.value <= 0.5 ? true : false;
        isComplete = false;

        if (target != null && weapon != null) {
            target.Value.GetComponent<WeaponController>().activateWeaponAttack(HANDKIND.right, true);

            if (horizontal) weapon.Value.GetComponent<Animator>().SetTrigger("attackHorizontal");
            else weapon.Value.GetComponent<Animator>().SetTrigger("attackVertical");
        }

        if (AnimationFinish.instance) AnimationFinish.instance.finish.AddListener(animationComplete);
    }

    public override TaskStatus OnUpdate() {
        if (isComplete) return TaskStatus.Success;
        else return TaskStatus.Running;
    }

    void animationComplete() {
        target.Value.GetComponent<WeaponController>().activateWeaponAttack(HANDKIND.right, false);
        isComplete = true;
    }
}