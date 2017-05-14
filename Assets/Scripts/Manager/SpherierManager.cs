using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpherierManager : MonoBehaviour
{
    #region Events
    private UnityEvent onIncreaseATK;
    private UnityEvent onIncreaseHP;
    private UnityEvent onIncreaseATKSpeed;

    private UnityEvent onUpgradeDash;
    private UnityEvent onUpgradeAngelicWeapon;
    private UnityEvent onUpgradeDemonicWeapon;
    #endregion

    public GameObject[] spherierTab;
    private int spherierIndex;
    private GameObject compSelected;

    delegate void FireCompEvent();
    private List<FireCompEvent> tabComp = new List<FireCompEvent>();

    void Awake()
    {
        onIncreaseATK = new UnityEvent();
        onIncreaseHP = new UnityEvent();
        onIncreaseATKSpeed = new UnityEvent();

        onUpgradeDash = new UnityEvent();
        onUpgradeAngelicWeapon = new UnityEvent();
        onUpgradeDemonicWeapon = new UnityEvent();
    }

	// Use this for initialization
	void Start () {
        spherierIndex = 0;
        GetObjectSelected();
        AddAllCompetences();
	}
	
	// Update is called once per frame
	void Update () {
        ChangingCompSelect();
        UpgradeComp();
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
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) tabComp[spherierIndex]();
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
        onIncreaseATK.Invoke();
        Debug.Log("Augmente l'ATK");
    }

    private void IncreaseHP()
    {
        onIncreaseHP.Invoke();
        Debug.Log("Augmente les HP");
    }

    private void IncreaseATKSpeed()
    {
        onIncreaseATKSpeed.Invoke();
        Debug.Log("Augmente l'ATKSpeed");
    }

    private void UpgradeDash()
    {

    }

    private void UpgradeAngelicWeapon()
    {

    }

    private void UpgradeDemonicWeapon()
    {

    }
    #endregion
}
