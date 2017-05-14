namespace Rpg
{
    using UnityEngine;
    using UnityEngine.Events;
    using Rpg;
    using System.Collections;

    public class GameManager : MonoBehaviour
    {
        private static GameManager m_Manager;
        public static GameManager manager { get { return m_Manager; } }
        public bool isReady { get; private set; }

        [Header("GameManager")]
        //Game events
        public BaseGameEvent onPlay;
        public BaseGameEvent onMenu;
        public BaseGameEvent onGameOver;

        //Game States
        private enum STATE { menu, play };
        private STATE m_State;

        public bool isPlaying { get { return m_State == STATE.play; } }
        public bool isMenu { get { return m_State == STATE.menu; } }


        void Awake()
        {
            m_Manager = this;

            onPlay = new BaseGameEvent();
            onMenu = new BaseGameEvent();
            onGameOver = new BaseGameEvent();

        }

        IEnumerator Start()
        {
            while (!MenuManager.manager && MenuManager.manager.isReady)
                yield return null;

            isReady = true;

            Menu();

            yield break;
        }

        void OnDestroy()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!isPlaying) return;
        }

        //Game States
        void Menu()
        {
            m_State = STATE.menu;
            if (onMenu != null) onMenu.Invoke();
        }

        void Play()
        {
            m_State = STATE.play;
            if (onPlay != null) onPlay.Invoke();

        }

        private void GameOver()
        {

        }
        

        //Callbacks
        public void PlayButtonHasBeenClicked()
        {
            Play();
        }

    }
}