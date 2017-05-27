using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg {

    /// <summary>
    /// 
    /// </summary>
    public class TextFile : MonoBehaviour {

        public TextAsset[] textFiles;

        private bool isTrigger = false;

        private int indexTextFile = 0;

        private bool canSendText = false;
        private bool canBeSend = false;

        // Use this for initialization
        void Start() {
            if (DialogueManager.instance && textFiles != null) {
                canSendText = true;
                DialogueManager.instance.endDialogue.AddListener(activePanel);
            }
        }

        void Update() {
            if (canBeSend && Input.GetButtonDown("AttackR") && indexTextFile < textFiles.Length - 1) {
                indexTextFile++;
                DialogueManager.instance.setNextTextFile(textFiles[indexTextFile]);
            }
        }

        void activePanel() {
            if (isTrigger) DialogueManager.instance.activePanelInteraction(true);
        }

        void OnTriggerEnter() {
            if (canSendText) {
                DialogueManager.instance.textFile = textFiles[indexTextFile];
                DialogueManager.instance.activePanelInteraction(true);
                canBeSend = true;
            }
        }

        void OnTriggerStay() {
            isTrigger = true;
        }

        void OnTriggerExit() {
            if (canSendText) {
                DialogueManager.instance.textFile = null;
                DialogueManager.instance.activePanelInteraction(false);
                DialogueManager.instance.setNextTextFile(null);
                isTrigger = false;
                canBeSend = false;
            }
        }
    }
}