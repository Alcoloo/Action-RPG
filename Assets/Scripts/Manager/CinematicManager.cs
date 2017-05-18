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
        public TextAsset textZone1;

        public GameObject[] pathCams;
        private int currentStep = 0;

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LaunchEnterParadise()
        {
            Debug.Log("Je suis entrée");
            Camera.current.GetComponent<ThirdPersonOrbitCam>().enabled = false;
            pathCams[1].GetComponent<CameraPathAnimator>().enabled = true;
            pathCams[1].GetComponent<CameraPathAnimator>().Play();
            RPGCharacterController.instance.StartCoroutine(RPGCharacterController.instance._EnterParadise());
        }

        public void PortalObjective()
        {
            DialogueManager.instance.setTextFile(textZone1);
            DialogueManager.instance.Init();
            //Debug.Log("Voilà l'entrée du Paradis");
        }
        public void EnemyTalk()
        {
            DialogueManager.instance.UpdateDialogueText();
            //Debug.Log("Attrapez la !");
        }
        public void LuciferTalk()
        {
            DialogueManager.instance.UpdateDialogueText();
            //Debug.Log("Olala je dois faire vite !");
        }

        public void EndZone1()
        {
            DialogueManager.instance.forceCloseDialogue();
            Camera.current.GetComponent<ThirdPersonOrbitCam>().enabled = true;
        }
    }
}


