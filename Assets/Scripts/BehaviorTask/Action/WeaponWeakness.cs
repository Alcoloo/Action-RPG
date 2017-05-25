using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Rpg.Characters;
using Rpg;

[TaskDescription("Specified the weakness of a GameObject")]
public class WeaponWeakness : Action
{

    [BehaviorDesigner.Runtime.Tasks.Tooltip("GameObject target")]
    public SharedGameObject target = null;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("Align Weakness")]
    public ALIGN align = ALIGN.both;

    public override void OnStart()
	{
        target.Value.GetComponent<Caracteristic>().setAlign(align);
    }

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}