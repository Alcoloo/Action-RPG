using Rpg.Manager;
using UnityEngine;

namespace Rpg.Characters
{

    /// <summary>
    /// 
    /// </summary>
    public class LevellingCarac : MonoBehaviour
    {
        [SerializeField]
        private int _expToEarn = 10;
        protected void Start()
        {
            if(LevellingManager.manager == null)
            {
                Debug.LogError("LevelManager doesn't exist");
            }
        }

        protected void Update()
        {

        }
        void OnDisable()
        {
            LevellingManager.manager.gainExp(_expToEarn);
        }
        void OnDestroyed()
        {
            LevellingManager.manager.gainExp(_expToEarn);
        }
    }
}