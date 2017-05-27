using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using Rpg;

namespace BehaviorDesigner.Runtime.Movement
{
    public class PatrolDetection : NavMeshMovement
    {
        public SharedBool randomPatrol = false;
        public SharedFloat waypointPauseDuration = 0;
        public SharedGameObjectList waypoints;

        // The current index that we are heading towards within the waypoints array
        private int waypointIndex;
        private float waypointReachedTime;

        private Enemy enemyScript;

        public override void OnStart()
        {
            enemyScript = GetComponent<Enemy>();
            base.OnStart();

            // initially move towards the closest waypoint
            float distance = Mathf.Infinity;
            float localDistance;
            for (int i = 0; i < waypoints.Value.Count; ++i)
            {
                if ((localDistance = Vector3.Magnitude(transform.position - waypoints.Value[i].transform.position)) < distance)
                {
                    distance = localDistance;
                    waypointIndex = i;
                }
            }
            waypointReachedTime = -1;
            SetDestination(Target());
            enemyScript.ChangeAnimationState("walk");
        }

        // Patrol around the different waypoints specified in the waypoint array. Always return a task status of running. 
        public override TaskStatus OnUpdate()
        {
            if (enemyScript.isDetected())
            {
                enemyScript.lookPlayer();
                return TaskStatus.Failure;
            }
            if (waypoints.Value.Count == 0)
            {
                return TaskStatus.Failure;
            }
            if (HasArrived())
            {
                if (waypointReachedTime == -1)
                {
                    waypointReachedTime = Time.time;
                }
                // wait the required duration before switching waypoints.
                if (waypointReachedTime + waypointPauseDuration.Value <= Time.time)
                {
                    if (randomPatrol.Value)
                    {
                        if (waypoints.Value.Count == 1)
                        {
                            waypointIndex = 0;
                        }
                        else
                        {
                            // prevent the same waypoint from being selected
                            var newWaypointIndex = waypointIndex;
                            while (newWaypointIndex == waypointIndex)
                            {
                                newWaypointIndex = Random.Range(0, waypoints.Value.Count);
                            }
                            waypointIndex = newWaypointIndex;
                        }
                    }
                    else
                    {
                        waypointIndex = (waypointIndex + 1) % waypoints.Value.Count;
                    }
                    SetDestination(Target());
                    waypointReachedTime = -1;
                }
            }

            return TaskStatus.Running;
        }

        // Return the current waypoint index position
        private Vector3 Target()
        {
            if (waypointIndex >= waypoints.Value.Count)
            {
                return transform.position;
            }
            return waypoints.Value[waypointIndex].transform.position;
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();

            randomPatrol = false;
            waypointPauseDuration = 0;
            waypoints = null;
        }

        // Draw a gizmo indicating a patrol 
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (waypoints == null || waypoints.Value == null)
            {
                return;
            }
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            for (int i = 0; i < waypoints.Value.Count; ++i)
            {
                if (waypoints.Value[i] != null)
                {
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
                    UnityEditor.Handles.SphereCap(0, waypoints.Value[i].transform.position, waypoints.Value[i].transform.rotation, 1);
#else
                    UnityEditor.Handles.SphereHandleCap(0, waypoints.Value[i].transform.position, waypoints.Value[i].transform.rotation, 1, EventType.Repaint);
#endif
                }
            }
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}
