using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Manager
{

    /// <summary>
    /// 
    /// </summary>
    public class ScenesManager : MonoBehaviour
    {

        private static ScenesManager _instance;

        private AsyncOperation async = null;

        public Texture2D emptyProgressBar;
        public Texture2D fullProgressBar;

        /// <summary>
        /// instance unique de la classe     
        /// </summary>
        public static ScenesManager instance
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
                throw new Exception("Tentative de création d'une autre instance de SceneManager alors que c'est un singleton.");
            }
            _instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }

        protected void Start()
        {

        }

        protected void Update()
        {
            OnGUI();
        }

        public void LoadNextScene(string pSceneName)
        {
            StartCoroutine(LoadALevel(pSceneName));
        }

        private IEnumerator LoadALevel(string pSceneName)
        {
            async = SceneManager.LoadSceneAsync(pSceneName);
            yield return async;
        }

        protected void OnDestroy()
        {
            _instance = null;
        }

        void OnGUI()
        {
            if (async != null)
            {
                GUI.DrawTexture(new Rect(0, 0, 100, 50), emptyProgressBar);
                GUI.DrawTexture(new Rect(0, 0, 100 * async.progress, 50), fullProgressBar);
            }
        }
    }
}