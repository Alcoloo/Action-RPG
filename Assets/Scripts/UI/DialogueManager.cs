using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject textBox;
    public GameObject panelInteraction;

    public TextMeshProUGUI theText;

    public TextAsset textFile;
    private TextAsset nextTextFile;
    public string[] textLines;

    public int speedText = 1;
    
    public int currentLine;
    public int endAtLine;

    public UnityEvent endDialogue;

    private bool isDialog = false;
    private bool coroutineIsFinish = true;

    private static DialogueManager _instance;

    /// <summary>
    /// instance unique de la classe     
    /// </summary>
    public static DialogueManager instance {
        get {
            return _instance;
        }
    }

    protected void Awake() {
        if (_instance != null) {
            throw new Exception("Tentative de création d'une autre instance de MonoBehaviorSingleton1 alors que c'est un singleton.");
        }
        _instance = this;

        endDialogue = new UnityEvent();
    }

    // Use this for initialization
    void Start () {
        speedText = 1 / speedText;
    }

    public void Init() {
        if (textFile != null) {

            isDialog = true;

            textLines = (textFile.text.Split('\n'));
            if (endAtLine == 0) {
                endAtLine = textLines.Length - 1;
            }
            currentLine = 0;

            UpdateDialogueText();

            activePanelInteraction(false);
            textBox.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update() {
        if (ControllerInput.manager.interact && !isDialog) {
            Init();
        }
        if (ControllerInput.manager.interact && coroutineIsFinish) {
            UpdateDialogueText();
        }
	}

    public void setNextTextFile(TextAsset pTextFile) {
        nextTextFile = pTextFile;
    }

    public void activePanelInteraction(bool pActive) {
        panelInteraction.SetActive(pActive);
    }

    public void UpdateDialogueText() {
        int currentPage = theText.pageToDisplay;
        
        if (currentPage < theText.textInfo.pageCount) {
            currentPage++;
        }
        else if (currentLine <= endAtLine) {
            currentPage = 1;

            string character = textLines[currentLine].Split(':')[0] + " :";
            string dialog = textLines[currentLine].Split(':')[1];

            theText.text = character;
            StartCoroutine(CoroutineText(dialog));
            currentLine++;
        }
        else {
            forceCloseDialogue();
            endDialogue.Invoke();
        }
    }

    public void forceCloseDialogue()
    {
        textBox.SetActive(false);
        activePanelInteraction(false);
        endAtLine = 0;

        isDialog = false;

        if (nextTextFile != null) textFile = nextTextFile;
    }
    
    IEnumerator CoroutineText(string pDialog) {
        coroutineIsFinish = false;

        char[] charArray = pDialog.ToCharArray();
        
        for (int i=0; i<pDialog.Length; i++) {
            theText.text += charArray[i];
            yield return new WaitForSeconds(speedText/10);
        }

        coroutineIsFinish = true;
    }

    protected void OnDestroy() {
        _instance = null;
    }

    public void setTextFile(TextAsset pTextFile)
    {
        textFile = pTextFile;
    }
}