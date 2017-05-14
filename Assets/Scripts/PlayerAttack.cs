using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {

    private string type;
    private int comboNumber;
    public Text comboText;
	// Use this for initialization
	void Start () {
        comboNumber = 0;
	}
	
	// Update is called once per frame
	void Update () {
        EquipWeapon();
	}

    public bool IsAttacking()
    {
        comboNumber++;
        comboText.text = "x"+ comboNumber.ToString();
        return true;
    }

    private void EquipWeapon()
    {
        int random = Random.Range(1, 3);
        if (random == 1) type = "Demon";
        else type = "Angel";
    }

    public string WeaponEquiped()
    {
        return type;
    }
}
