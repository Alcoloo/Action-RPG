using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Rpg;

namespace Rpg
{

    /// <summary>
    /// 
    /// </summary>
    public class ScenesManager : BaseManager<ScenesManager>
    {

        private static ScenesManager _instance;

        private AsyncOperation async = null;

        private GameObject loadingCanvas;
        public Image loadingBar;
        public Text loadingTxt;

        private float progress;

        public string[] sceneOrder;

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

        protected override void Awake()
        {
            base.Awake();
            loadingCanvas = transform.Find("CanvasLoading").gameObject;

        }

        protected void Start()
        {

        }

        protected void Update()
        {

        }

        public void changeScene()
        {
            LoadNextScene(getNextScene(SceneManager.GetActiveScene().name));
        }
        public void reloadScene()
        {
            LoadNextScene(SceneManager.GetActiveScene().name);
        }

        public void LoadNextScene(string pSceneName)
        {
            loadingCanvas.SetActive(true);
            if(PoolingManager.manager != null) PoolingManager.manager.resetPool();
            StartCoroutine(LoadALevel(pSceneName));
        }

        private IEnumerator LoadALevel(string pSceneName)
        {
            async = SceneManager.LoadSceneAsync(pSceneName);
            while(!async.isDone)
            {
                progress = Mathf.Clamp01(async.progress / 0.9f);
                loadingBar.fillAmount = progress;
                loadingTxt.text = "Loading: " + (progress * 100) + "%";
                yield return null;  
            }
            loadingCanvas.SetActive(false);
        }

        private string getNextScene(string pCurrentScene)
        {
            for(int i = 0; i < sceneOrder.Length; i++)
            {
                if(pCurrentScene == sceneOrder[i])
                {
                    if (sceneOrder[i + 1] != "") return sceneOrder[i + 1];
                }
            }
            return "";
        }
    }
}