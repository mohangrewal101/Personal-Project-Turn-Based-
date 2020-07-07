using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHudScript : MonoBehaviour
{
    public GameObject hpBar;
    public Text nameText;
    public Text levelText;

    //REQUIRES: Unit to not be null
    //MODIFIES: 
    //EFFECTS: Sets the hud of the specified unit with name, level, and hp
    public void SetHUD(UnitScript unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl" + unit.unitLevel;
    //  hpBar.maxValue = unit.maxHP / 100;
        hpBar.transform.localScale = new Vector3 (unit.currentHP / 100, 1);
    }

    public void SetHP(float hp)
    {
        hpBar.transform.localScale = new Vector3(hp / 100, 1);
    }
}
