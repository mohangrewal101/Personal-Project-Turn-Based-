using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [HideInInspector]
    public bool hasAttacked = false;
    [HideInInspector]
    public bool canHighlightTile = true;
    [HideInInspector]
    public bool entityHasMovedToHighlightedTile = true;
    [HideInInspector]
    public Vector3Int entityTilePosition;
    [HideInInspector]
    public Vector3Int highlightedTilePosition;
    [HideInInspector]
    public Vector3Int originalTilePosition;

    public Transform entityTransform;

    public GameObject entity;

    public PlayerInventory inventory;

    public UnitScript unitScript;

    public BattleHudScript hud;

    public Text UIText;

    public Animator anim;

    public TextFadeOut textFadeOut;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
