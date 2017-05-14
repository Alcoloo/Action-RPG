using UnityEngine;

public class Boss : Enemy {

    public TextAsset textFile;

    void Start() {
        Init();
    }

    virtual protected void Init() {
        Debug.Log("Bonjour");
        InitDialogue();
    }

    virtual protected void InitDialogue() {
        if(textFile!=null) {
            DialogueManager.instance.textFile = textFile;
            DialogueManager.instance.Init();
        }
    }

    virtual protected void InitPhase() {

    }

    virtual protected void EndPhase() {

    }
}