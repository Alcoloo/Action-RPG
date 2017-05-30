using Assets.Scripts.Boss.Gabriel;
using BehaviorDesigner.Runtime;
using Rpg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class CinematicManager : MonoBehaviour
    {
        // Use this for initialization
        private static CinematicManager _instance;
        public static CinematicManager instance { get { return _instance; } }
        public Camera cam;
        public List<TextAsset> textZone;
        public GameObject[] GabrielDoors;
        private IEnumerator activeCoroutine;
        public GameObject[] pathCams;
        private int currentStep = 0;
        private Vector3 startPlayerPos;
        delegate void Cinematic();
        List<Cinematic> cinematicStep = new List<Cinematic>();
        private GameObject portal;
        private bool isFadeOver = false;

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            cinematicStep.Add(GabrielEnterCinematic);
            cinematicStep.Add(StartGabrielDeath);
            //EnableCam();
            portal = GameObject.FindGameObjectWithTag("portal");
            //paradiseDoor.GetComponent<ParadiseDoor>().onPlayerEnter.AddListener(LaunchEnterParadise);
        }

        public void LaunchCinematic()
        {
            EnableCam();
            cinematicStep[currentStep]();
        }

        private void EnableCam()
        {
            cam.GetComponent<ThirdPersonOrbitCam>().enabled = false;
            pathCams[currentStep].GetComponent<CameraPathAnimator>().enabled = true;
            pathCams[currentStep].GetComponent<CameraPathAnimator>().Play();
        }

        private void GabrielEnterCinematic()
        {
            startPlayerPos = Player.instance.transform.position;
            activeCoroutine = RPGCharacterController.instance._GabrielCinematic();
            StartCoroutine(activeCoroutine);      
        }

        public void LaunchEnterParadise()
        {
            Debug.Log("Je suis entrée");
            RPGCharacterController.instance.StartCoroutine(RPGCharacterController.instance._EnterParadise());
        }

        #region pathEvents
        public void PortalObjective()
        {
            DialogueManager.instance.setTextFile(textZone[currentStep]);
            DialogueManager.instance.Init();
        }

        public void EnemyTalk()
        {
            DialogueManager.instance.UpdateDialogueText();
        }

        public void LuciferTalk()
        { 
            DialogueManager.instance.UpdateDialogueText();
        }

        public void EndZone1()
        {
            DialogueManager.instance.forceCloseDialogue();
            HudManager.manager.ChangeHUDVisibility();
            cam.GetComponent<ThirdPersonOrbitCam>().enabled = true;
        }

        #region Gabriel
        #region GabrielEnter
        public void EndGabrielCinematic()
        {
            pathCams[currentStep].GetComponent<CameraPathAnimator>().Pause();
            StopCoroutine(activeCoroutine);
            RPGCharacterController.instance.SetPropertiesToDefault();
            DialogueManager.instance.setTextFile(textZone[currentStep]);
            DialogueManager.instance.Init();
            if (DialogueManager.instance != null) DialogueManager.instance.endDialogue.AddListener(StartGabrielFight);
            DialogueManager.instance.forceCloseDialogue();
        }

        public void StartGabrielCinematic()
        {
            portal.SetActive(false);
            RPGCharacterController.instance.SetModeCinematic();

            Player.instance.SetModeCinematic();

            Gabriel.instance.GetComponent<BehaviorTree>().enabled = false;
            ThirdPersonOrbitCam camComponent = cam.GetComponent<ThirdPersonOrbitCam>();
            camComponent.currentBoss = Gabriel.instance.transform;
            //camComponent.SetModeBoss();
            HudManager.manager.ChangeHUDVisibility();
        }

        public void StartGabrielFight()
        {
            DialogueManager.instance.endDialogue.RemoveListener(StartGabrielFight);
            pathCams[currentStep].GetComponent<CameraPathAnimator>().Play();
        }

        public void BlackScreenCameraStart()
        {
            StartCoroutine(WaitForFadeTime(true));
            DisableCam();
        }

        private IEnumerator WaitForFadeTime(bool needReset)
        {
            float fadeTime = cam.GetComponent<Fading>().BeginFade(1);
            yield return new WaitForSeconds(fadeTime);
            cam.GetComponent<Fading>().BeginFade(-1);
            yield return new WaitForSeconds(fadeTime / 6);
            if (needReset) ResetPlayerAndGabriel();
        }

        private void ResetPlayerAndGabriel()
        {
            Player.instance.transform.position = startPlayerPos;
            Gabriel.instance.GetComponent<BehaviorTree>().enabled = true;
            HudManager.manager.ChangeHUDVisibility();
        }

        private void DisableCam()
        {
            RPGCharacterController.instance.rpgCharacterState = RPGCharacterState.DEFAULT;
            RPGCharacterController.instance.SetModeDefault();
            cam.GetComponent<ThirdPersonOrbitCam>().enabled = true;
            pathCams[currentStep].GetComponent<CameraPathAnimator>().enabled = false;
            currentStep++;
        }

        #endregion
        #region Gabriel Death
        private void StartGabrielDeath()
        {
            RPGCharacterController.instance.SetModeCinematic();
            Player.instance.SetModeCinematic();
            Gabriel.instance.GetComponent<BehaviorTree>().enabled = false;
            StartCoroutine(WaitForFadeTime(false));
            HudManager.manager.ChangeHUDVisibility();
            EnableCam();
        }

        private void PauseDeathPath()
        {
            pathCams[currentStep].GetComponent<CameraPathAnimator>().Pause();
            DialogueManager.instance.setTextFile(textZone[currentStep]);
            DialogueManager.instance.Init();
            if (DialogueManager.instance != null) DialogueManager.instance.endDialogue.AddListener(LaunchPortalPath);
        }

        private void LaunchPortalPath()
        {
            portal.SetActive(true);
            pathCams[currentStep].GetComponent<CameraPathAnimator>().Play();
        }

        private void PlayAfterGabrielDeath()
        {
            for (int i = 0; i < GabrielDoors.Length; i++)
            {
                GabrielDoors[i].GetComponent<Animator>().SetTrigger("isTrigger");
            }
        }

        private void EndGabrielDeathCinematic()
        {
            StartCoroutine(WaitForFadeTime(false));
            DisableCam();
        }
        #endregion
        #endregion
        #endregion
    }
}
