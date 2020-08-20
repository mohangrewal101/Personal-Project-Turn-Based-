using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
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

    private bool isPlayerHealing;
    [HideInInspector]
    private bool isEnemyAttacking;
    private bool isPlayerAttacking;
    private bool playerUsedTurn;
    private bool readyToSpawn;
    private bool canSpawn;
    private bool isThereMoreEnemiesSpawned;

    private float itemWidth = 29;
    private float itemHeight = 7;
    private float itemDistancePosy = 9;
    private float item1Posx = -16;
    private float item1Posy = 13;

    private int numOfItemTypes = 0; //number of items types in inventory
    private int xSpawn;
    private int ySpawn;

    private List<GameObject> itemPositions = new List<GameObject>(); 
    
    [HideInInspector]
    public UnitScript playerUnit;
    [HideInInspector]
    public UnitScript enemyUnit;

    private Queue<GameObject> turnQueue; //queue for the next turn

    private GameObject[] unitsToSpawn;
    [HideInInspector]
    public GameObject playerGO;
    [HideInInspector]
    public GameObject enemyGO;
    [HideInInspector]
    public GameObject magicOptions;
    private GameObject itemOptions;
    private TileBase[] tileArray;

    // public GameObject playerPrefab;
    // public GameObject enemyPrefab;
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
    public Tilemap tileMap;

    private BoundsInt tileBounds;

    //  private Text enemyUItext;
    //   private Text playerUItext;
    public Text dialogueText;

  //  private TextFadeOut enemyUItextFadeOut;
  //  private TextFadeOut playerUItextFadeOut;


   // private Animator enemyAnim;
    private Animator playerAnim;
 //   private BattleHudScript playerHUD;
   // private BattleHudScript enemyHUD;
    public PlayerChoosingTiles playerChoosingTiles;
    public EnemyChoosingTiles enemyChoosingTiles;
    private PlayerInventory playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        tileMap.CompressBounds();
        tileBounds = tileMap.cellBounds;
        tileArray = tileMap.GetTilesBlock(tileMap.cellBounds);
        state = BattleState.START;
        playerChoosingTiles.numOfUnitsToSpawn = Random.Range(playerChoosingTiles.minNumOfUnitsThatCanSpawn, playerChoosingTiles.maxNumOfUnitsThatCanSpawn + 1);
        enemyChoosingTiles.numOfUnitsToSpawn = Random.Range(enemyChoosingTiles.minNumOfUnitsThatCanSpawn, enemyChoosingTiles.maxNumOfUnitsThatCanSpawn + 1);
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
       Object[] objUnitsToSpawn = Resources.LoadAll("Characters", typeof(GameObject));
        unitsToSpawn = objUnitsToSpawn.Cast<GameObject>().ToArray<GameObject>();
        int numOfEnemiesSoFar = 0;
        for (int i = 0; i < unitsToSpawn.Length; i++)
        {
            if (unitsToSpawn[i].tag == "Player")
            {
                //int x = Random.Range(minTileInScene, maxTileInScene);
                readyToSpawn = true;
                yield return new WaitUntil(() => canSpawn == true);
                canSpawn = false;
                readyToSpawn = false;
                playerGO = Instantiate(unitsToSpawn[i], tileMap.GetCellCenterWorld(new Vector3Int(xSpawn, (int) (ySpawn + 1.5f), 0)), Quaternion.identity);
                playerGO.transform.SetParent(playerBattleStation);
                Settings playerSettings = playerGO.GetComponent<Settings>();
                playerSettings.entityTilePosition = new Vector3Int(xSpawn, ySpawn, 0);
                playerSettings.hud.SetHUD(playerSettings.unitScript);
                playerSettings.UIText.text = "";
                playerChoosingTiles.spawnedUnits.Add(playerGO);
            }
            else if (numOfEnemiesSoFar < enemyChoosingTiles.numOfUnitsToSpawn && unitsToSpawn[i].tag == "Enemy")
            {
                //int x = Random.Range(minTileInScene, maxTileInScene);
                readyToSpawn = true;
                yield return new WaitUntil(() => canSpawn == true);
                canSpawn = false;
                readyToSpawn = false;
                enemyGO = Instantiate(unitsToSpawn[i], tileMap.GetCellCenterWorld(new Vector3Int(xSpawn, (int)(ySpawn + 1.5f), 0)), Quaternion.identity);
                enemyGO.transform.SetParent(enemyBattleStation);
                Settings enemySettings = enemyGO.GetComponent<Settings>();
                enemySettings.entityTilePosition = new Vector3Int(xSpawn, ySpawn, 0);
                dialogueText.text = enemySettings.unitScript.unitName + " is coming to attack!!!";
                enemySettings.hud.SetHUD(enemySettings.unitScript);
                enemySettings.UIText.text = "";
                enemyChoosingTiles.spawnedUnits.Add(enemyGO);
                numOfEnemiesSoFar++;
            }
        }
        CreateQueue();
        if (numOfEnemiesSoFar >= 2) isThereMoreEnemiesSpawned = true;

        if (turnQueue.Peek().tag == "Player")
            state = BattleState.PLAYERTURN;
        else if (turnQueue.Peek().tag == "Enemy")
        {
            DisableButtons();
        }

            yield return new WaitForSeconds(2f);
        if (turnQueue.Peek().tag == "Player")
            PlayerTurn();
        else if (turnQueue.Peek().tag == "Enemy")
        {

            state = BattleState.ENEMYTURN;

            StartCoroutine(EnemyTurn());
        }
    }

    bool IsTileFree(int x, int y)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x + 0.5f, y), Vector3.up, 30f, ~LayerMask.GetMask("Ground"));
       // Debug.DrawRay(new Vector2(x + 0.5f, y), Vector3.up * 30f, Color.red, 10);
        return hit.collider == null;
    }

    //Creates a turn queue with the spawned units from both spawned players and spawned enemies in the current scene
    void CreateQueue()
    {
        List<GameObject> spawnedUnits = new List<GameObject>();
        spawnedUnits.AddRange(playerChoosingTiles.spawnedUnits);
        spawnedUnits.AddRange(enemyChoosingTiles.spawnedUnits);
        spawnedUnits.Sort(SpeedSort);
        spawnedUnits.Reverse();
        turnQueue = new Queue<GameObject>(spawnedUnits);        
    }

    int SpeedSort(GameObject a, GameObject b)
    {
        return a.transform.GetChild(0).GetComponent<UnitScript>().speed.CompareTo(b.transform.GetChild(0).GetComponent<UnitScript>().speed);
    }

    void PlayerTurn()
    {
        playerGO = turnQueue.Dequeue();
        dialogueText.text = "Choose an option.";
        if (playerGO.GetComponent<Settings>().unitScript.buffTurnsLeft == 0) playerGO.GetComponent<Settings>().unitScript.unitDefenseBuff = 0;
        else playerGO.GetComponent<Settings>().unitScript.buffTurnsLeft--;
    }

    private void Update()
    {
        if (readyToSpawn)
        {
            xSpawn = Random.Range(tileMap.origin.x, tileMap.size.x + tileMap.origin.x);
            ySpawn = Random.Range(tileMap.origin.y, tileMap.size.y + tileMap.origin.y);
            canSpawn = IsTileFree(xSpawn, ySpawn);
        }
        UpdateAnimations();
    }
    void UpdateAnimations()
    {
       
        playerGO.GetComponent<Settings>().anim.SetBool("isPlayerAttacking", isPlayerAttacking);
        playerGO.GetComponent<Settings>().anim.SetBool("isPlayerHealing", isPlayerHealing);
        enemyGO.GetComponent<Settings>().anim.SetBool("isenemyMovingToHighlightedTile", !enemyGO.GetComponent<Settings>().entityHasMovedToHighlightedTile);
        playerGO.GetComponent<Settings>().anim.SetBool("isPlayerMovingToHighlightedTile", !playerGO.GetComponent<Settings>().entityHasMovedToHighlightedTile);
        playerGO.GetComponent<Settings>().anim.SetBool("isEnemyAttacking", isEnemyAttacking);
    }

    void SetUpPlayerAttack()
    {
        if (Mathf.Round(Mathf.Abs(playerGO.GetComponent<Settings>().entityTilePosition.x - enemyGO.GetComponent<Settings>().entityTilePosition.x)) > 1 || playerGO.GetComponent<Settings>().entityTilePosition != playerGO.GetComponent<Settings>().highlightedTilePosition)
            playerGO.GetComponent<Settings>().entityHasMovedToHighlightedTile = false;
        playerGO.GetComponent<Settings>().hasAttacked = false;
    }

    void NextInQueue()
    {
        if (turnQueue.Peek().tag == "Player")
        {
            EnableButtons();
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        else if (turnQueue.Peek().tag == "Enemy")
        {
            DisableButtons();
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    void ChooseBetweenEnemies()
    {
        
    }

    public IEnumerator PlayerHeal()
    {
        DisableButtons();
        SetUpPlayerAttack();
        yield return new WaitUntil(() => playerGO.GetComponent<Settings>().entityHasMovedToHighlightedTile == true);
        isPlayerHealing = true;

        yield return new WaitForSeconds(1.1f);
        playerGO.GetComponent<Settings>().unitScript.Heal(10);
        playerGO.GetComponent<Settings>().hud.SetHP(playerGO.GetComponent<Settings>().unitScript.currentHP);
        dialogueText.text = "You used ancient arts to heal!";

        playerChoosingTiles.mouseButtonPressed = false;
        playerGO.GetComponent<Settings>().hasAttacked = true;
        yield return new WaitForSeconds(1.5f);
        isPlayerHealing = false;
        EnableButtons();
        turnQueue.Enqueue(playerGO);

        NextInQueue();
        Destroy(magicOptions);
    }
    public IEnumerator PlayerAttack()
    {
        DisableButtons();
        SetUpPlayerAttack();
        yield return new WaitUntil(() => playerGO.GetComponent<Settings>().entityHasMovedToHighlightedTile == true);
        isPlayerAttacking = true;
        bool isDead = false;
        //  GameObject[] spawnedUnits = turnQueue.ToArray();
        // if (isThereMoreEnemiesSpawned) ChooseBetweenEnemies();
        if (Mathf.Abs(playerGO.GetComponent<Settings>().entityTilePosition.x - enemyGO.GetComponent<Settings>().entityTilePosition.x) > playerGO.GetComponent<Settings>().unitScript.meleeRange)
        {
            dialogueText.text = "The attack fails!";
            enemyGO.GetComponent<Settings>().UIText.text = "MISS";
            enemyGO.GetComponent<Settings>().textFadeOut.FadeOut();


        } else {

            isDead = enemyGO.GetComponent<Settings>().unitScript.TakeDamage(playerGO.GetComponent<Settings>().unitScript.damage);
            enemyGO.GetComponent<Settings>().anim.SetTrigger("hit");
            dialogueText.text = "The attack is successful";
            enemyGO.GetComponent<Settings>().UIText.text = "HIT";
            enemyGO.GetComponent<Settings>().textFadeOut.FadeOut();
        }

        enemyGO.GetComponent<Settings>().hud.SetHP(enemyGO.GetComponent<Settings>().unitScript.currentHP);
        playerGO.GetComponent<Settings>().hasAttacked = true;
        playerChoosingTiles.mouseButtonPressed = false;

        yield return new WaitForSeconds(0.7f);
        isPlayerAttacking = false;
        isPlayerHealing = false;

        enemyGO.GetComponent<Settings>().UIText.text = "";
        enemyGO.GetComponent<Settings>().textFadeOut.FixText();
        turnQueue.Enqueue(playerGO);
        if (isDead)
        {
            enemyGO.GetComponent<Settings>().anim.Play("SkeletonDeath");
            state = BattleState.WON;
            EndBattle();
        } else
        {
            enemyGO.GetComponent<Settings>().anim.SetBool("isDead", isDead);
            NextInQueue();
        }
    }

    public IEnumerator EnemyTurn()
    {
        enemyGO = turnQueue.Dequeue();
        enemyGO.GetComponent<Settings>().entityHasMovedToHighlightedTile = false;
        enemyGO.GetComponent<Settings>().hasAttacked = false;

        
        yield return new WaitUntil(() => enemyGO.GetComponent<Settings>().entityHasMovedToHighlightedTile == true);
        bool isDead = false;
        isEnemyAttacking = true;
        if (Mathf.Abs(playerGO.GetComponent<Settings>().entityTilePosition.x - enemyGO.GetComponent<Settings>().entityTilePosition.x) > enemyGO.GetComponent<Settings>().unitScript.meleeRange)
        {
            dialogueText.text = enemyGO.GetComponent<Settings>().unitScript.name + " misses!";
            playerGO.GetComponent<Settings>().UIText.text = "MISS";
            playerGO.GetComponent<Settings>().textFadeOut.FadeOut();

        }
        else
        {
            dialogueText.text = enemyGO.GetComponent<Settings>().unitScript.name + " attacks!";
            isDead = playerGO.GetComponent<Settings>().unitScript.TakeDamage(enemyGO.GetComponent<Settings>().unitScript.damage);
            playerGO.GetComponent<Settings>().anim.SetTrigger("hit");
            playerGO.GetComponent<Settings>().UIText.text = "HIT";
            playerGO.GetComponent<Settings>().textFadeOut.FadeOut();
        }
        enemyGO.GetComponent<Settings>().hasAttacked = true;
        playerGO.GetComponent<Settings>().hud.SetHP(playerGO.GetComponent<Settings>().unitScript.currentHP);
        yield return new WaitForSeconds(1.05f);
        isEnemyAttacking = false;
        playerGO.GetComponent<Settings>().UIText.text = "";
        playerGO.GetComponent<Settings>().textFadeOut.FixText();
        enemyGO.GetComponent<Settings>().hasAttacked = false;
        turnQueue.Enqueue(enemyGO);

        if (isDead)
        {
            playerGO.GetComponent<Settings>().anim.Play("PlayerDeath");
            state = BattleState.LOST;
            EndBattle();
        } else
        {
            NextInQueue();
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

        yield return new WaitUntil(() => playerGO.GetComponent<Settings>().entityHasMovedToHighlightedTile == true);

        bool isDead = false;
        if (Mathf.Abs(playerGO.GetComponent<Settings>().entityTilePosition.x - enemyGO.GetComponent<Settings>().entityTilePosition.x) > playerGO.GetComponent<Settings>().unitScript.magicRange)
        {
            dialogueText.text = "The fire misses!";
            enemyGO.GetComponent<Settings>().UIText.text = "MISS";
            enemyGO.GetComponent<Settings>().textFadeOut.FadeOut();
        } else
        {
            GameObject fireObject = Instantiate(playerGO.GetComponent<Settings>().unitScript.rangedAttackPrefab, playerGO.GetComponent<Settings>().unitScript.firePoint);
            FireScript fire = fireObject.GetComponent<FireScript>();
            yield return new WaitUntil(() => fire.hasFireCollided == true);
            Destroy(fireObject);
            dialogueText.text = "Fire hits!";
            isDead = enemyGO.GetComponent<Settings>().unitScript.TakeDamage(playerGO.GetComponent<Settings>().unitScript.damage);
            enemyGO.GetComponent<Settings>().anim.SetTrigger("hit");
            enemyGO.GetComponent<Settings>().UIText.text = "HIT";
            enemyGO.GetComponent<Settings>().textFadeOut.FadeOut();
        }
        enemyGO.GetComponent<Settings>().hud.SetHP(enemyGO.GetComponent<Settings>().unitScript.currentHP);
        playerGO.GetComponent<Settings>().hasAttacked = true;
        playerChoosingTiles.mouseButtonPressed = false;

        yield return new WaitForSeconds(2f);
        enemyGO.GetComponent<Settings>().UIText.text = "";
        enemyGO.GetComponent<Settings>().textFadeOut.FixText();
        turnQueue.Enqueue(playerGO);
        if (isDead)
        {
            enemyGO.GetComponent<Settings>().anim.Play("SkeletonDeath");
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            enemyGO.GetComponent<Settings>().anim.SetBool("isDead", isDead);
            NextInQueue();
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
        if (playerGO.GetComponent<Settings>().inventory.inventoryItems.Capacity != 0)
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
        itemText.text = i.itemName + " x" + playerGO.GetComponent<Settings>().inventory.itemNumbers[i];
        itemButton.onClick.AddListener(delegate { ItemFunction(itemInOptions, i); }); //adds button using item function and ITEMBUTTONSCRIPT
    }

    public void ItemFunction(GameObject itemInOptions, Item i)
    {
        switch (i.itemType)
        {
            case Item.Type.HEALING:
                playerGO.GetComponent<Settings>().unitScript.Heal(10);
                break;
            case Item.Type.BUFF:
                playerGO.GetComponent<Settings>().unitScript.DefenseBuff(10, 2);
                break;
            case Item.Type.ATTACK:
                enemyGO.GetComponent<Settings>().unitScript.TakeDamage(10);
                break;
        }
        playerGO.GetComponent<Settings>().inventory.removeItem(i);
        Destroy(itemInOptions);
        Destroy(itemOptions);
        turnQueue.Enqueue(playerGO);
        NextInQueue();
    }

}
