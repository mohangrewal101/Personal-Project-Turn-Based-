using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerandEnemyStats : MonoBehaviour
{
    private UnitScript unit;

    private GameObject unitUIObject;

    public GameObject statsPrefab;
    public Transform unitBattleStation;
    public Sprite unitSprite;

    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<UnitScript>();
    }

    public void OnUnitButton()
    {
        Debug.Log("Button clicked");
        if (unitUIObject == null)
        {
            unitUIObject = Instantiate(statsPrefab ,unitBattleStation);
            InstantiateStats();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && unitUIObject)
            DestroyObject(unitUIObject);
    }

    void InstantiateStats()
    {
        unitUIObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = unit.unitName;
        unitUIObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = "HP: " + unit.currentHP + "/" + unit.maxHP;
        unitUIObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Attack: " + unit.damage;
        unitUIObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Lvl " + unit.unitLevel;
        unitUIObject.transform.GetChild(4).gameObject.GetComponent<Image>().sprite = unitSprite;
    }
}
