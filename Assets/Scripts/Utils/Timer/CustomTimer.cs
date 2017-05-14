using UnityEngine;
using System;

namespace Assets.Scripts.Utils.Timer
{

    /// <summary>
    /// 
    /// </summary>
    public class CustomTimer : MonoBehaviour
    {

        private static CustomTimer _instance;

        #region Time Variables
        /// temps ecoulé depuis le debut de la partie

        public float elapsedTime { get; private set; }

        protected bool isPaused = true;
        #endregion
        /// <summary>
        /// instance unique de la classe     
        /// </summary>
        public static CustomTimer instance
        {
            get
            {
                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance != null)
            {
                throw new Exception("Tentative de création d'une autre instance de CustomTimer alors que c'est un singleton.");
            }
            _instance = this;
        }



        protected void Start()
        {
            elapsedTime = 0;
            isPaused = false;
        }

        #region Timer
        protected void Update()
        {
            if (!isPaused)
            {
                elapsedTime += Time.deltaTime;
            }
        }
        #endregion

        public bool isTime(float pStartTime, float pDuration)
        {
            return (elapsedTime - pStartTime) >= pDuration;
        }

        public void stopTimer()
        {
            isPaused = true;
        }

        public void startTimer()
        {
            isPaused = false;
        }

        protected void OnDestroy()
        {
            _instance = null;
        }
    }
}