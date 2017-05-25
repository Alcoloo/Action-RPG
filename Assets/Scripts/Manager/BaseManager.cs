namespace Rpg
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.Events;

    [System.Serializable]
    public class BaseGameEvent : UnityEvent
    {

    }

    public abstract class BaseManager<T> : Singleton<T> where T : Component
    {
        [HideInInspector]
        public bool isReady { get; protected set; }

        protected override void Awake()
        {
            base.Awake();
            isReady = false;
        }

        protected virtual void OnDestroy()
        {
            if (GameManager.manager)
            {
                GameManager.manager.onMenu.RemoveListener(Menu);
                GameManager.manager.onPlay.RemoveListener(Play);
                GameManager.manager.onGameOver.RemoveListener(GameOver);
            }
        }

        void Start()
        {
            if (GameManager.manager)
            {
                GameManager.manager.onMenu.AddListener(Menu);
                GameManager.manager.onPlay.AddListener(Play);
                GameManager.manager.onGameOver.AddListener(GameOver);
            }
            //else Debug.LogError("BaseManager " + name + " tells you: NO GameManager");
            
        }

        // Update is called once per frame
        protected virtual void Play()
        {
        }

        protected virtual void Menu()
        {
        }

        protected virtual void GameOver()
        {
        }

    }
}
