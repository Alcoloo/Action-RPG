using Rpg;
using UnityEngine;

namespace Assets.Scripts.Utils
{

    /// <summary>
    /// 
    /// </summary>
    public class DeathFlag : MonoBehaviour
    {

        protected void Start()
        {

        }

        protected void Update()
        {

        }
        public void OnTriggerEnter( Collider col)
        {
            ScenesManager.manager.reloadScene();
        }
    }
}