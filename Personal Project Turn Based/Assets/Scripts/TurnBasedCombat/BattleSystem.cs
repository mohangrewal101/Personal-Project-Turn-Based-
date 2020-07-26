﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BattleState
{
    START, PLAYERTURN, ENEMYTURN, WON, LOST
}
public class BattleSystem : MonoBehaviour
{
    public class Max
    {
        public int max = 8;
    }

    [HideInInspector]
    public bool hasPlayerAttacked;
    private bool isPlayerHealing;
    [HideInInspector]
    public bool hasEnemyAttacked;
    private bool isEnemyAttacking;
    private bool isPlayerAttacking;
    private bool playerUsedTurn;

    private float itemWidth = 29;
    private float itemHeight = 7;
    private float itemDistancePosy = 9;
    private float item1Posx = -16;
    private float item1Posy = 13;

    private int numOfItemTypes = 0; //number of items types in inventory

    private List<GameObject> itemPositions = new List<GameObject>(); 
    
    [HideInInspector]
    public UnitScript playerUnit;
    [HideInInspector]
    public UnitScript enemyUnit;

    private GameObject playerGO;
    private GameObject enemyGO;
    [HideInInspector]
    public GameObject magicOptions;
    private GameObject itemOptions;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject magicOptionsPrefab;
    public GameObject itemOptionsPrefab;
    public GameObject itemButtonPrefab;

    public Button attackButton;
    public Button magicButton;
    public Button itemButton;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public Transform magicBattleStation;
    public Transform itemBattleStation;
    public BattleState state;

    private Text enemyUItext;
    private Text playerUItext;
    public Text dialogueText;

    private TextFadeOut enemyUItextFadeOut;
    private TextFadeOut playerUItextFadeOut;


    private Animator enemyAnim;
    private Animator playerAnim;
    private BattleHudScript playerHUD;
    private BattleHudScript enemyHUD;
    public PlayerChoosingTiles playerChoosingTiles;
    public EnemyChoosingTiles enemyChoosingTiles;
    private PlayerInventory playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());
    }

    void DestroyItemOptions()
    {
        if (itemOptions)
            Destroy(itemOptions);
    }

    void DestroyMagicOptions()
    {
        if (magicOptions)
            Destroy(magicOptions);
    }
    void DisableButtons()
    {
        attackButton.interactable = false;
        magicButton.interactable = false;
        itemButton.interactable = false;
        if (magicOptions)
        {
            magicOptions.transform.FindChild("Heal").gameObject.GetComponent<Button>().interactable = false;
            magicOptions.transform.FindChild("Fire").gameObject.GetComponent<Button>().interactable = false;
        }
    }

    void EnableButtons()
    {
        attackButton.interactable = true;
        magicButton.interactable = true;
        itemButton.interactable = true;

    }

    // Sets up the battle in the game
    IEnumerator SetUpBattle()
    {
        playerGO = Instantiate(playerPrefab, playerBattleStation); //playerPrefab is a child of the playerBattleStation
        playerInventory = playerGO.transform.GetChild(0).GetComponent<PlayerInventory>();
        playerUnit = playerGO.transform.GetChild(0).GetComponent<UnitScript>();
        enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.transform.GetChild(0).GetComponent<UnitScript>();
        dialogueText.text = enemyUnit.unitName + " is coming to attack!!!";
        playerHUD = playerGO.transform.GetChild(2).GetComponent<BattleHudScript>();
        playerHUD.SetHUD(playerUnit);
        enemyHUD = enemyGO.transform.GetChild(1).GetComponent<BattleHudScript>();
        enemyHUD.SetHUD(enemyUnit);
        enemyUItext = GameObject.Find("EnemyBattleStation").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();
        playerUItext = GameObject.Find("PlayerBattleStation").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();
        enemyUItext.text = "";
        playerUItext.text = "";
        enemyAnim = GameObject.Find("EnemyBattleStation").transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>();
        playerAnim = GameObject.Find("PlayerBattleStation").transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>();
        enemyUItextFadeOut = GameObject.Find("EnemyBattleStation").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<TextFadeOut>();
        playerUItextFadeOut = GameObject.Find("PlayerBattleStation").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<TextFadeOut>();
        state = BattleState.PLAYERTURN;

        yield return new WaitForSeconds(2f);
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an option.";
    }

    private void Update()
    {
        UpdateAnimations();
    }
    void UpdateAnimations()
    {
        playerAnim.SetBool("isPlayerAttacking", isPlayerAttacking);
        playerAnim.SetBool("isPlayerHealing", isPlayerHealing);
        enemyAnim.SetBool("isenemyMovingToHighlightedTile", enemyChoosingTiles.isEntityMovingToHighlightedTile);
        playerAnim.SetBool("isPlayerMovingToHighlightedTile", !playerChoosingTiles.entityHasMovedToHighlightedTile);
        enemyAnim.SetBool("isEnemyAttacking", isEnemyAttacking);
    }

    void SetUpPlayerAttack()
    {
        if (Mathf.Round(Mathf.Abs(playerChoosingTiles.entityTilePosition.x - enemyChoosingTiles.entityTilePosition.x)) > 1 || playerChoosingTiles.entityTilePosition != playerChoosingTiles.highlightedTilePosition)
            playerChoosingTiles.entityHasMovedToHighlightedTile = false;
        hasPlayerAttacked = false;
    }

    public IEnumerator PlayerHeal()
    {
        DisableButtons();
        SetUpPlayerAttack();
        yield return new WaitUntil(() => playerChoosingTiles.entityHasMovedToHighlightedTile == true);
        isPlayerHealing = true;

        yield return new WaitForSeconds(1.1f);
        playerUnit.Heal(10);
        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You used ancient arts to heal!";

        playerChoosingTiles.mouseButtonPressed = false;
        hasPlayerAttacked = true;
        yield return new WaitForSeconds(1.5f);
        isPlayerHealing = false;
        EnableButtons();
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
        Destroy(magicOptions);
    }
    public IEnumerator PlayerAttack()
    {
        DisableButtons();
        SetUpPlayerAttack();
        yield return new WaitUntil(() => playerChoosingTiles.entityHasMovedToHighlightedTile == true);
        isPlayerAttacking = true;
        bool isDead = false;

        if (Mathf.Abs(playerChoosingTiles.entityTilePosition.x - enemyChoosingTiles.entityTilePosition.x) > playerUnit.meleeRange)
        {
            dialogueText.text = "The attack fails!";
            enemyUItext.text = "MISS";
            enemyUItextFadeOut.FadeOut();


        } else {

            isDead = enemyUnit.TakeDamage(playerUnit.damage);
            enemyAnim.SetTrigger("hit");
            dialogueText.text = "The attack is successful";
            enemyUItext.text = "HIT";
            enemyUItextFadeOut.FadeOut();
        }

        enemyHUD.SetHP(enemyUnit.currentHP);
        hasPlayerAttacked = true;
        playerChoosingTiles.mouseButtonPressed = false;

        yield return new WaitForSeconds(0.7f);
        isPlayerAttacking = false;
        isPlayerHealing = false;
        
        enemyUItext.text = "";
        enemyUItextFadeOut.FixText();
        EnableButtons();
        if (isDead)
        {
            enemyAnim.Play("SkeletonDeath");
            state = BattleState.WON;
            EndBattle();
        } else
        {
            enemyAnim.SetBool("isDead", isDead);
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public IEnumerator EnemyTurn()
    {
        enemyChoosingTiles.isEntityMovingToHighlightedTile = true;
        enemyChoosingTiles.entityHasMovedToHighlightedTile = false;
        hasEnemyAttacked = false;
        
        yield return new WaitUntil(() => enemyChoosingTiles.entityHasMovedToHighlightedTile == true);
        bool isDead = false;
        isEnemyAttacking = true;
        if (Mathf.Abs(playerChoosingTiles.entityTilePosition.x - enemyChoosingTiles.entityTilePosition.x) > enemyUnit.meleeRange)
        {
            dialogueText.text = enemyUnit.name + " misses!";
            playerUItext.text = "MISS";
            playerUItextFadeOut.FadeOut();

        }
        else
        {
            dialogueText.text = enemyUnit.name + " attacks!";
            isDead = playerUnit.TakeDamage(enemyUnit.damage);
            playerAnim.SetTrigger("hit");
            playerUItext.text = "HIT";
            playerUItextFadeOut.FadeOut();
        }
        hasEnemyAttacked = true;
        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(1.05f);
        isEnemyAttacking = false;
        playerUItext.text = "";
        playerUItextFadeOut.FixText();
        hasEnemyAttacked = false;

        if (isDead)
        {
            playerAnim.Play("PlayerDeath");
            state = BattleState.LOST;
            EndBattle();
        } else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
            Destroy(enemyGO, 1.2f);
        } else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
            Destroy(playerGO, 1.0f);
        }
    }
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        DestroyMagicOptions();
        DestroyItemOptions();
        StartCoroutine(PlayerAttack());
    }

    public void OnMagicButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        DestroyItemOptions();
        if (magicOptions == null)
            magicOptions = Instantiate(magicOptionsPrefab, magicBattleStation);
        else
            DestroyMagicOptions();
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        PlayerHeal();
    }

    public IEnumerator PlayerFire()
    {
        DisableButtons();
        MagicAttacks firemagicAttack = GameObject.Find("Fire").GetComponent<MagicAttacks>();
        SetUpPlayerAttack();

        yield return new WaitUntil(() => playerChoosingTiles.entityHasMovedToHighlightedTile == true);

        bool isDead = false;
        if (Mathf.Abs(playerChoosingTiles.entityTilePosition.x - enemyChoosingTiles.entityTilePosition.x) > playerUnit.magicRange)
        {
            dialogueText.text = "The fire misses!";
            enemyUItext.text = "MISS";
            enemyUItextFadeOut.FadeOut();
        } else
        {
            GameObject fireObject = Instantiate(playerUnit.rangedAttackPrefab, playerUnit.firePoint);
            FireScript fire = fireObject.GetComponent<FireScript>();
            yield return new WaitUntil(() => fire.hasFireCollided == true);
            Destroy(fireObject);
            dialogueText.text = "Fire hits!";
            isDead = enemyUnit.TakeDamage(playerUnit.damage);
            enemyAnim.SetTrigger("hit");
            enemyUItext.text = "HIT";
            enemyUItextFadeOut.FadeOut();
        }
        enemyHUD.SetHP(enemyUnit.currentHP);
        hasPlayerAttacked = true;
        playerChoosingTiles.mouseButtonPressed = false;

        yield return new WaitForSeconds(2f);
        enemyUItext.text = "";
        enemyUItextFadeOut.FixText();
        EnableButtons();
        if (isDead)
        {
            enemyAnim.Play("SkeletonDeath");
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            enemyAnim.SetBool("isDead", isDead);
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
            Destroy(magicOptions);
        }
    }

    /// <summary>
    /// UNFINISHED CODE FOR ITEMS IS BELOW
    /// </summary>
    /// 


    //If in Player Turn battle state and itemOptions is null, instantiate itemOptionsPrefab and player items on to 
    // item battle station
    public void OnItemButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        DestroyMagicOptions();
        DestroyItemOptions();
        if (itemOptions == null)
        {
            itemOptions = Instantiate(itemOptionsPrefab, itemBattleStation);
            InstantiateItems();
        }
        else Destroy(itemOptions);
    }


    //Instantiates the items in player's inventory on to the itemOptions
    public void InstantiateItems()
    {
        if (playerInventory.inventoryItems.Capacity != 0)
        {
            foreach (Item i in playerInventory.inventoryItems)
            {
                if (numOfItemTypes == 0)
                {
                    IntiliazeItem(i, 0f);
                } else
                {
                    IntiliazeItem(i, 9f);
                }
            }
        }
    }

    //Initiliazes how item will look like in unity
    public void IntiliazeItem(Item i, float unitsAway)
    {
        GameObject itemInOptions = null;
        switch (i.itemType)
        {
            case Item.Type.HEALING:
                itemInOptions = Instantiate(itemButtonPrefab, itemBattleStation.GetChild(0).GetChild(0));
                break;
            case Item.Type.BUFF:
                itemInOptions = Instantiate(itemButtonPrefab, itemBattleStation.GetChild(0).GetChild(1));
                break;
            case Item.Type.ATTACK:
                itemInOptions = Instantiate(itemButtonPrefab, itemBattleStation.GetChild(0).GetChild(2));
                break;
        }
        RectTransform itemPosition = itemInOptions.GetComponent<RectTransform>();
        itemPosition.sizeDelta = new Vector2(itemWidth, itemHeight);
        Text itemText = itemInOptions.GetComponentInChildren<Text>();
        Button itemButton = itemInOptions.GetComponent<Button>();
        itemText.text = i.itemName + " x" + playerInventory.itemNumbers[i];
        itemButton.onClick.AddListener(delegate { ItemFunction(itemInOptions, i); }); //adds button using item function and ITEMBUTTONSCRIPT
    }

    public void ItemFunction(GameObject itemInOptions, Item i)
    {
        switch (i.itemType)
        {
            case Item.Type.HEALING:
                playerUnit.Heal(10);
                break;
            case Item.Type.BUFF:
                playerUnit.DefenseBuff(10);
                break;
            case Item.Type.ATTACK:
                enemyUnit.TakeDamage(10);
                break;
        }
        playerInventory.removeItem(i);
        Destroy(itemInOptions);
        Destroy(itemOptions);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

}
