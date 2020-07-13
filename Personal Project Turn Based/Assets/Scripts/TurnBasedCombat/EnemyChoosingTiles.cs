using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyChoosingTiles : ChoosingTiles
{
    [HideInInspector]
    private bool canHighlightTile;

    [HideInInspector]
    public bool isEnemyTurn; //CHECK IF IT'S ENEMY'S TURN


    // Start is called before the first frame update
    private void Start()
    {
        canHighlightTile = true;
        entityHasMovedToHighlightedTile = true;
        entityTilePosition = new Vector3Int((int) 6.5, 0, 0); //TEMP
        entityTransform = GameObject.FindGameObjectWithTag("Enemy").transform.GetChild(0).transform;
        entity = GameObject.FindGameObjectWithTag("Enemy").transform.GetChild(0).GetChild(0).gameObject;


    }
    // Update is called once per frame
    void Update()
    {
        CheckMovementDirection();
        if (!isEntityMovingToHighlightedTile && entityHasMovedToHighlightedTile) //When should tile be unhighlighted?
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
        if (!battleSystem.hasEnemyAttacked && !entityHasMovedToHighlightedTile)
        {
            MoveToTile();
        }
    }

    override protected void HighlightTile()
    {
        Vector3Int tilePosition = opposingChoosingTiles.entityTilePosition;
        Transform enemyPosition = GameObject.Find("EnemyBattleStation").transform.FindChild("Skeleton(Clone)").transform;
        Transform playerPosition = GameObject.Find("PlayerBattleStation").transform.FindChild("Player1(Clone)").transform;
        if (Mathf.Round(Mathf.Abs(entityTilePosition.x - opposingChoosingTiles.entityTilePosition.x)) > 1)
        {
            if (Mathf.Round(enemyPosition.position.x) < Mathf.Round(playerPosition.position.x))
            {
                if (movementDirection == 0)
                {
                    isFacingOtherWay = !isFacingOtherWay;
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
                    isFacingOtherWay = !isFacingOtherWay;
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

    override protected void MoveToTile()
    {

        Transform enemyTransform = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
        if (isEntityMovingToHighlightedTile && enemyTransform.position.x != highlightedTilePosition.x + 0.5)
        {
            if (enemyTransform.position.x < highlightedTilePosition.x + 0.5)
            {
                enemyTransform.Translate(Vector2.right * tileTravelSpeed * Time.deltaTime);
                if (enemyTransform.position.x >= highlightedTilePosition.x + 0.5)
                {
                    MovingToTiles();
                    canHighlightTile = true;
                }

            }
            else if (enemyTransform.position.x > highlightedTilePosition.x + 0.5)
            {
                enemyTransform.Translate(-Vector2.right * tileTravelSpeed * Time.deltaTime);
                if (enemyTransform.position.x <= highlightedTilePosition.x + 0.5)
                {
                    MovingToTiles();
                    canHighlightTile = true;
                }

            }

        }

    }
}
