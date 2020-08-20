using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerandEnemyStats : MonoBehaviour
{
    private UnitScript unit;

    private GameObject unitUIObject;
    private GameObject unitEffectBubble;

    public GameObject statsPrefab;
    public GameObject effectPrefab;
    public Transform unitBattleStation;
    public Transform effectBattleStation;
    public Sprite unitSprite;
    public Sprite effectSprite;

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
        if (Input.GetMouseButtonDown(0) && unitUIObject) DestroyObject(unitUIObject);

        if (unitEffectBubble == null && unit.buffTurnsLeft != 0)
        {
            unitEffectBubble = Instantiate(effectPrefab, effectBattleStation);
            InstantiateEffectBubble();
        }else if (unit.buffTurnsLeft == 0) DestroyObject(unitEffectBubble);
    }

    void InstantiateStats()
    {
        unitUIObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = unit.unitName;
        unitUIObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = "HP: " + unit.currentHP + "/" + unit.maxHP;
        unitUIObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Attack: " + unit.damage;
        unitUIObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Lvl " + unit.unitLevel;
        unitUIObject.transform.GetChild(4).gameObject.GetComponent<Image>().sprite = unitSprite;
    }

    void InstantiateEffectBubble()
    {
        unitEffectBubble.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = effectSprite;
    }
}
