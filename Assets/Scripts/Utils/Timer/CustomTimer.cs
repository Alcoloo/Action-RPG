using UnityEngine;
using System;
using System.Collections;

namespace Rpg
{

    /// <summary>
    /// 
    /// </summary>
    public class CustomTimer : BaseManager<CustomTimer>
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

        protected override void Awake()
        {
            base.Awake();

        }

        protected override IEnumerator CoroutineStart()
        {
            throw new NotImplementedException();
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
    }
}