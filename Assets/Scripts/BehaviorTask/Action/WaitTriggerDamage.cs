using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Rpg.Characters;

[TaskDescription("Wait a specified amount of time. If there is a trigger Faillure.")]
[TaskIcon("{SkinColor}WaitIcon.png")]
public class WaitTriggerDamage : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The amount of time to wait")]
    public SharedFloat waitTime = 1;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("The amount of damage to trigger")]
    public SharedInt damage = 10;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("GameObject who take damage")]
    public SharedGameObject target = null;

    // The time to wait
    private float waitDuration;
    // The time that the task started to wait.
    private float startTime;
    // Remember the time that the task is paused so the time paused doesn't contribute to the wait time.
    private float pauseTime;

    // Life of the target
    private int life;
    // Life of the target at the begining
    private int startLife;

    public override void OnStart() {
        startTime = Time.time;
        waitDuration = waitTime.Value;

        if (target != null) {
            life = target.Value.GetComponent<Caracteristic>().pv;
            startLife = life;
        }
    }

    public override TaskStatus OnUpdate()
	{

        if (target != null) life = target.Value.GetComponent<Caracteristic>().pv;

        if (life <= startLife - damage.Value) {
            return TaskStatus.Failure;
        }
        // The task is done waiting if the time waitDuration has elapsed since the task was started.
        else if (startTime + waitDuration < Time.time) {
            return TaskStatus.Success;
        }
        // Otherwise we are still waiting.
        return TaskStatus.Running;
    }

    public override void OnPause(bool paused) {
        if (paused) {
            // Remember the time that the behavior was paused.
            pauseTime = Time.time;
        }
        else {
            // Add the difference between Time.time and pauseTime to figure out a new start time.
            startTime += (Time.time - pauseTime);
        }
    }

    public override void OnReset() {
        // Reset the public properties back to their original values
        waitTime = 1;
    }
}