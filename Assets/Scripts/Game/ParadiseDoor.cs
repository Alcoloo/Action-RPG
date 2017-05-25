using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using UnityEngine.Events;

namespace Assets.Scripts.Game
{
    class ParadiseDoor: MonoBehaviour
    {
        public UnityEvent onPlayerEnter;
        protected void Start()
        {

        }
        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "player") onPlayerEnter.Invoke();
        }
    }
}
