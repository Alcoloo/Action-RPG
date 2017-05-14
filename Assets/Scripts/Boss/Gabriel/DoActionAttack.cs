using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Boss.Gabriel
{
   
    /// <summary>
    /// 
    /// </summary>
    public class DoActionAttack : Action
    {
        public float angle;
        public float radius;

        

        protected void Start()
        {
            float t;
            for (int i = 0; i < 10; i++)
            {
                t = i / (10 - 1);
                LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.SetPosition(i, GetQuadraticCoordinates(t));
            }
        }

        void OnGUI()
        {
            Debug.Log("go");
            Handles.color = new Color(1, 0, 0, 1);
            Handles.DrawSolidArc(transform.position, Vector3.up, Vector3.right, angle, radius);
            Handles.DrawSolidArc(transform.position, Vector3.up, Vector3.right, -angle, radius);
            Handles.color = Color.black;
        }

        Vector3 GetQuadraticCoordinates(float t)
        {
            return Mathf.Pow(1-t,2)*Vector3.right + 2*t*(1-t)*Vector3.up + Mathf.Pow(t,2)*Vector3.left;
        }

    protected void Update()
        {

        }
    }
}