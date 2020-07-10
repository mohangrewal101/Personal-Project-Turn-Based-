using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyChoosingTiles : MonoBehaviour
{
    [HideInInspector]
    public bool isenemyMovingToHighlightedTile;
    private bool canHighlightTile;
    private bool isFacingPlayer;

    [HideInInspector]
    public bool isEnemyTurn; //CHECK IF IT'S ENEMY'S TURN
    [HideInInspector]
    public bool enemyHasMovedToHighlightedTile;

    public int tileTravelSpeed;

    public int numberofTilesToMoveOn;

    private Vector3Int highlightedTilePosition;
    private Vector3Int originalTilePostion;

    [HideInInspector]
    public Vector3Int enemyTilePosition;

    private Vector3 worldPoint;
    private int movementDirection;
    private Transform enemyTransform;
    public Tilemap groundTilemap;
    public TileBase highlightedTile;
    public TileBase nonHighlightedTile;
    public BattleSystem battleSystem;
    public ChoosingTiles playerChoosingTiles;


    // Start is called before the first frame update
    private void Start()
    {
        battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        canHighlightTile = true;
        enemyHasMovedToHighlightedTile = true;
        enemyTilePosition = new Vector3Int((int) 6.5, 0, 0); //TEMP
        isFacingPlayer = false;
        movementDirection = 1;
        enemyTransform = GameObject.Find("EnemyBattleStation").transform.GetChild(0).GetChild(0).transform;


    }
    // Update is called once per frame
    void Update()
    {
        CheckMovementDirection();
        if (!isenemyMovingToHighlightedTile && enemyHasMovedToHighlightedTile) //When should tile be unhighlighted?
        {
            UnHighlightTile();
            originalTilePostion = highlightedTilePosition; //makes the originalTilePosition = to the highlighted position
        }

        isEnemyTurn = (battleSystem.state == BattleState.ENEMYTURN);

        if (isEnemyTurn && !battleSystem.hasEnemyAttacked && canHighlightTile) 
        {
            canHighlightTile = false;
            UnHighlightTile();
            originalTilePostion = highlightedTilePosition; 
            HighlightTile();
        }
        if (!battleSystem.hasEnemyAttacked && !enemyHasMovedToHighlightedTile)
        {
            MoveToTile();
        }
    }

    void CheckMovementDirection()
    {
        if (isFacingPlayer)
            Flip();
    }

    //EFFECTS: Rotates the sprite 180 degrees on the y-axis
    void Flip()
    {
        isFacingPlayer = !isFacingPlayer;
        enemyTransform.Rotate(0.0f, 180.0f, 0.0f);
        GameObject child = GameObject.FindGameObjectWithTag("Enemy").transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        GameObject child2 = GameObject.FindGameObjectWithTag("Enemy").transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        Vector3 childScale = child.transform.localScale; //switches back UI
        childScale.x *= -1;
        child.transform.localScale = childScale;

        Vector3 childScale2 = child2.transform.localScale; //switches back UI
        childScale2.x *= -1;
        child2.transform.localScale = childScale2;
    }

    //REQUIRES: tile to be highlighted
    //MODIFIES: this
    //EFFECTS: Makes a tile not highlighted anymore, if it's position is not equal to the position
    // of the highlightd tile
    void UnHighlightTile()
    {
        if (originalTilePostion != highlightedTilePosition)
        {
            groundTilemap.SetTile(originalTilePostion, nonHighlightedTile);
        }
    }

    //REQUIRES: tile to not be highlighted
    //MODIFIES: this
    //EFFECTS: Highlights tile if it's not already highlighted anymore
    void HighlightTile()
    {
        Vector3Int tilePosition = playerChoosingTiles.playerTilePosition;
        Transform enemyPosition = GameObject.Find("EnemyBattleStation").transform.FindChild("Skeleton(Clone)").transform;
        Transform playerPosition = GameObject.Find("PlayerBattleStation").transform.FindChild("Player1(Clone)").transform;
        if (Mathf.Round(Mathf.Abs(enemyTilePosition.x - playerChoosingTiles.playerTilePosition.x)) > 1)
        {
            if (Mathf.Round(enemyPosition.position.x) < Mathf.Round(playerPosition.position.x))
            {
                if (movementDirection == 0)
                {
                    isFacingPlayer = !isFacingPlayer;
                    movementDirection = 1;
                }
                if (Mathf.Floor(Mathf.Abs(playerPosition.position.x - enemyPosition.position.x)) <= numberofTilesToMoveOn)
                {
                    tilePosition = new Vector3Int(tilePosition.x - 1, tilePosition.y, tilePosition.z);
                }
                else
                {
                    tilePosition = new Vector3Int(tilePosition.x - (int)Mathf.Floor(Mathf.Abs(playerPosition.position.x - enemyPosition.position.x) - numberofTilesToMoveOn), tilePosition.y, tilePosition.z);
                }
            }
            else
            {
                if (movementDirection == 1)
                {
                    isFacingPlayer = !isFacingPlayer;
                    movementDirection = 0;
                }
                if (Mathf.Floor(Mathf.Abs(playerPosition.position.x - enemyPosition.position.x)) <= numberofTilesToMoveOn)
                {
                    tilePosition = new Vector3Int(tilePosition.x + 1, tilePosition.y, tilePosition.z);
                }
                else
                {

                    tilePosition = new Vector3Int(tilePosition.x + (int)Mathf.Floor(Mathf.Abs(playerPosition.position.x - enemyPosition.position.x) - numberofTilesToMoveOn), tilePosition.y, tilePosition.z);
                }
            }
            if (highlightedTilePosition != tilePosition)
            {
                highlightedTilePosition = tilePosition;
                groundTilemap.SetTile(highlightedTilePosition, highlightedTile);
            }
        }
    }

    //REQUIRES: Player is attacking
    //MODIFES: this
    //EFFECTS: Player moves to highlighted tile if mouse button is clicked
    public void MoveToTile()
    {

        Transform enemyTransform = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
        if (isenemyMovingToHighlightedTile && enemyTransform.position.x != highlightedTilePosition.x + 0.5)
        {
            if (enemyTransform.position.x < highlightedTilePosition.x + 0.5)
            {
                enemyTransform.Translate(Vector2.right * tileTravelSpeed * Time.deltaTime);
                if (enemyTransform.position.x >= highlightedTilePosition.x + 0.5)
                {
                    enemyTilePosition = new Vector3Int(highlightedTilePosition.x, highlightedTilePosition.y, highlightedTilePosition.z);
                    UnHighlightTile();
                    isenemyMovingToHighlightedTile = false;
                    enemyHasMovedToHighlightedTile = true;
                    canHighlightTile = true;
                }

            }
            else if (enemyTransform.position.x > highlightedTilePosition.x + 0.5)
            {
                enemyTransform.Translate(-Vector2.right * tileTravelSpeed * Time.deltaTime);
                if (enemyTransform.position.x <= highlightedTilePosition.x + 0.5)
                {
                    enemyTilePosition = new Vector3Int(highlightedTilePosition.x, highlightedTilePosition.y, highlightedTilePosition.z);
                    UnHighlightTile();
                    isenemyMovingToHighlightedTile = false;
                    enemyHasMovedToHighlightedTile = true;
                    canHighlightTile = true;
                }

            }

        }

    }
}
