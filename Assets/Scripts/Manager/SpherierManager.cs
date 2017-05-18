using Rpg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Competences : MonoBehaviour
{
    public bool isActivated;
    public bool canBeActivated;
    public bool isLastComp;
    public string type;

}
public class onAtkUpgradeEvent : UnityEvent<float> { }
public class onHPUpgradeEvent : UnityEvent<float> { }
public class onArmorUpgradeEvent : UnityEvent<float> { }

public class SpherierManager : MonoBehaviour
{
    #region Events
    private onAtkUpgradeEvent onIncreaseATK;
    private onHPUpgradeEvent onIncreaseHP;
    private onArmorUpgradeEvent onIncreaseATKSpeed;

    private UnityEvent onUpgradeDash;
    private UnityEvent onUpgradeAngelicWeapon;
    private UnityEvent onUpgradeDemonicWeapon;
    #endregion

    public GameObject[] spherierTab;
    private int spherierIndex;
    private GameObject compSelected;

    delegate void FireCompEvent();
    private List<FireCompEvent> tabComp = new List<FireCompEvent>();

    private Dictionary<Competences, float> compStats = new Dictionary<Competences, float>();

    private Stats statsJSON = new Stats();

    

    void Awake()
    {
        onIncreaseATK = new onAtkUpgradeEvent();
        onIncreaseHP = new onHPUpgradeEvent();
        onIncreaseATKSpeed = new onArmorUpgradeEvent();

        onUpgradeDash = new UnityEvent();
        onUpgradeAngelicWeapon = new UnityEvent();
        onUpgradeDemonicWeapon = new UnityEvent();
    }

	// Use this for initialization
	void Start () {
        spherierIndex = 0;
        statsJSON.InitStats();
        GetObjectSelected();
        AddAllCompetences();
        InitAllCompetences();
	}
	
	// Update is called once per frame
	void Update () {
        ChangingCompSelect();
        UpgradeComp();
	}

    private void InitAllCompetences()
    {
        InitCompetences("AtkUpgrade");
        InitCompetences("HPUpgrade");
        InitCompetences("ArmorUpgrade");
    }

    private void InitCompetences(string tagName)
    {
        List<GameObject> compGO = PickRightTaggedComps(tagName);

        for (int i = 0; i < compGO.Count; i++)
        {
            compGO[i].AddComponent<Competences>();
            Competences comp = compGO[i].GetComponent<Competences>();
            comp.isActivated = false;
            comp.type = tagName;
            if (i == 0) comp.canBeActivated = true;
            else comp.canBeActivated = false;
            if (i == compGO.Count - 1) comp.isLastComp = true;
            else comp.isLastComp = false;
            compStats.Add(comp, statsJSON.GetBasicStatsStep(tagName, i));
        }
        
    }

    private List<GameObject> PickRightTaggedComps(string rightTag)
    {
        List<GameObject> rightComps = new List<GameObject>();
        for (int i = 0; i < spherierTab.Length; i++)
        {
            if (spherierTab[i].tag == rightTag) rightComps.Add(spherierTab[i]);
        }
        return rightComps;
    } 

    private void ChangingCompSelect()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            spherierIndex++;
            if (spherierIndex >= spherierTab.Length) spherierIndex = 0;
            GetObjectSelected();
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            spherierIndex--;
            if (spherierIndex < 0) spherierIndex = spherierTab.Length-1;
            GetObjectSelected();
        }
        
    }

    private void UpgradeComp()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Competences spherierComp = spherierTab[spherierIndex].GetComponent<Competences>();
            if (spherierComp.canBeActivated && !spherierComp.isActivated)
            {
                if (!spherierComp.isLastComp) spherierTab[spherierIndex + 1].GetComponent<Competences>().canBeActivated = true;
                if (spherierIndex+1 % 2 != 0) tabComp[spherierIndex / 2]();
                else tabComp[spherierIndex]();
                spherierComp.isActivated = true;
            }
        }
         
    }

    private void AddAllCompetences()
    {
        tabComp.Add(IncreaseATK);
        tabComp.Add(IncreaseHP);
        tabComp.Add(IncreaseATKSpeed);

        tabComp.Add(UpgradeDash);
        tabComp.Add(UpgradeAngelicWeapon);
        tabComp.Add(UpgradeDemonicWeapon);
    }

    private void GetObjectSelected()
    {
        compSelected = spherierTab[spherierIndex];
        Debug.Log(compSelected);
    }

    #region Competences Function
    private void IncreaseATK()
    {
        Competences rightComp = spherierTab[spherierIndex].GetComponent<Competences>();
        Debug.Log(compStats[rightComp]);
        onIncreaseATK.Invoke(compStats[rightComp]);
        Debug.Log("Augmente l'ATK");
    }

    private void IncreaseHP()
    {
        Competences rightComp = spherierTab[spherierIndex].GetComponent<Competences>();
        Debug.Log(compStats[rightComp]);
        onIncreaseHP.Invoke(compStats[rightComp]);
        Debug.Log("Augmente les HP");
    }

    private void IncreaseATKSpeed()
    {
        Competences rightComp = spherierTab[spherierIndex].GetComponent<Competences>();
        Debug.Log(compStats[rightComp]);
        onIncreaseATKSpeed.Invoke(compStats[rightComp]);
        Debug.Log("Augmente l'Armure");
    }

    private void UpgradeDash()
    {
        Debug.Log("bug");
    }

    private void UpgradeAngelicWeapon()
    {

    }

    private void UpgradeDemonicWeapon()
    {

    }
    #endregion
}
