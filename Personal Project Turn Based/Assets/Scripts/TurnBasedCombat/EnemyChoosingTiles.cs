using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyChoosingTiles : ChoosingTiles
{
   // private bool canHighlightTile;

    [HideInInspector]
    public bool isEnemyTurn; //CHECK IF IT'S ENEMY'S TURN



    // Start is called before the first frame update
    private void Start()
    {

        currUnit = battleSystem.enemyGO;

    }
    // Update is called once per frame
    void Update()
    {
        if (currUnit != battleSystem.enemyGO)
        {
            currUnit = battleSystem.enemyGO;
        }
        CheckMovementDirection();
        if (battleSystem.enemyGO.GetComponent<Settings>().entityHasMovedToHighlightedTile) //When should tile be unhighlighted?
        {
            UnHighlightTile();
            battleSystem.enemyGO.GetComponent<Settings>().originalTilePosition = battleSystem.enemyGO.GetComponent<Settings>().highlightedTilePosition; //makes the originalTilePosition = to the highlighted position
        }

        isEnemyTurn = (battleSystem.state == BattleState.ENEMYTURN);
        if (isEnemyTurn && !battleSystem.enemyGO.GetComponent<Settings>().hasAttacked && battleSystem.enemyGO.GetComponent<Settings>().canHighlightTile) 
        {
            
            battleSystem.enemyGO.GetComponent<Settings>().canHighlightTile = false;
            UnHighlightTile();
            battleSystem.enemyGO.GetComponent<Settings>().originalTilePosition = battleSystem.enemyGO.GetComponent<Settings>().highlightedTilePosition; 
            HighlightTile();
            battleSystem.enemyGO.GetComponent<Settings>().entityHasMovedToHighlightedTile = false;
        }
        if (!battleSystem.enemyGO.GetComponent<Settings>().hasAttacked && !battleSystem.enemyGO.GetComponent<Settings>().entityHasMovedToHighlightedTile) //make bool variables specific to each enemy!!!!
        {
            MoveToTile();
        }
    }

    override protected void HighlightTile()
    {
        Vector3Int tilePosition = battleSystem.playerGO.GetComponent<Settings>().entityTilePosition;
        Transform enemyPosition = battleSystem.enemyGO.transform;
        Transform playerPosition = battleSystem.playerGO.transform;
        
        if (Mathf.Round(Mathf.Abs(currUnit.GetComponent<Settings>().entityTilePosition.x - battleSystem.playerGO.GetComponent<Settings>().entityTilePosition.x)) > 1)
        {
            int distanceBtwnPlyrAndEnemy = (int) Mathf.Floor(Mathf.Abs(playerPosition.position.x - enemyPosition.position.x));
            if (Mathf.Round(enemyPosition.position.x) < Mathf.Round(playerPosition.position.x))
            {
                if (movementDirection == 0)
                {
                    isFacingOtherWay = !isFacingOtherWay;
                    movementDirection = 1;
                }
                int x = 0;
                if (distanceBtwnPlyrAndEnemy <= numberofTilesToMoveOn)
                    x = isThereAFreeTile(Add, enemyPosition.position, distanceBtwnPlyrAndEnemy);
                else
                    x = isThereAFreeTile(Add, enemyPosition.position, numberofTilesToMoveOn);

                tilePosition = new Vector3Int((int)Mathf.Floor(enemyPosition.position.x) + x, tilePosition.y, tilePosition.z);
                 
/*                if (Mathf.Floor(Mathf.Abs(playerPosition.position.x - enemyPosition.position.x)) <= numberofTilesToMoveOn)
                {
*//*                    Debug.Log("Player tile Position: " + playerPosition.position.x);
                    Debug.Log("Tile position: " + tilePosition.x);
                    Debug.Log("Enemy tile Position: " + enemyPosition.position.x);*//*
                    int offset = 0;
                    int x = EntityOnTile(Subtract, tilePosition, offset, true);
*//*                    Debug.Log("Enemy pos < Player pos # of tiles moved (less than numOfTiles): " + x);
                    Debug.Log("Tile Position to reach: " + (tilePosition.x - x));*//*
                    tilePosition = new Vector3Int(tilePosition.x - x, tilePosition.y, tilePosition.z);
                }
                else
                {

*//*                    Debug.Log("Player tile Position: " + playerPosition.position.x);
                    Debug.Log("Tile position: " + tilePosition.x);
                    Debug.Log("Enemy tile Position: " + enemyPosition.position.x);*//*
                    int offset = (int)Mathf.Floor(Mathf.Abs(playerPosition.position.x - enemyPosition.position.x) - numberofTilesToMoveOn);*/
                   // int x = isThereAFreeTile(Add, enemyPosition.position);
/*                    Debug.Log("Enemy pos < Player pos # of tiles moved: " + x);
                    Debug.Log("Tile Position to reach: " + (tilePosition.x - x));*/
                  //  tilePosition = new Vector3Int((int) Mathf.Floor(enemyPosition.position.x) + x, tilePosition.y, tilePosition.z);
              //  }
            }
            else
            {
                if (movementDirection == 1)
                {
                    isFacingOtherWay = !isFacingOtherWay;
                    movementDirection = 0;
                }
                int x = 0;
                if (distanceBtwnPlyrAndEnemy <= numberofTilesToMoveOn) 
                    x = isThereAFreeTile(Subtract, enemyPosition.position, distanceBtwnPlyrAndEnemy);
                else 
                    x = isThereAFreeTile(Subtract, enemyPosition.position, numberofTilesToMoveOn);

                tilePosition = new Vector3Int((int)Mathf.Floor(enemyPosition.position.x) - x, tilePosition.y, tilePosition.z);
/*                if (Mathf.Floor(Mathf.Abs(playerPosition.position.x - enemyPosition.position.x)) <= numberofTilesToMoveOn)
                {
                    Debug.Log("Player tile Position: " + playerPosition.position.x);
                    Debug.Log("Tile position: " + tilePosition.x);
                    Debug.Log("Enemy tile Position: " + enemyPosition.position.x);
                    int offset = 0;
                    int x = EntityOnTile(Add, tilePosition, offset, true);
                    Debug.Log("Enemy pos > Player pos # of tiles moved (less than numOfTiles): " + x);
                    Debug.Log("Tile Position to reach: " + (tilePosition.x + x));
                    tilePosition = new Vector3Int(tilePosition.x + x, tilePosition.y, tilePosition.z);
                }
                else
                {
                    Debug.Log("Player tile Position: " + playerPosition.position.x);
                    Debug.Log("Tile position: " + tilePosition.x);
                    Debug.Log("Enemy tile Position: " + enemyPosition.position.x);
                    int offset = (int)Mathf.Floor(Mathf.Abs(playerPosition.position.x - enemyPosition.position.x) - numberofTilesToMoveOn);*/
                   // int x = isThereAFreeTile(Subtract, enemyPosition.position);
/*                    Debug.Log("Enemy pos > Player pos # of tiles moved: " + x);
                    Debug.Log("Tile Position to reach: " + (tilePosition.x + x));*/
                  //  tilePosition = new Vector3Int((int) Mathf.Floor(enemyPosition.position.x) - x, tilePosition.y, tilePosition.z);
               // }
            }
            if (battleSystem.enemyGO.GetComponent<Settings>().highlightedTilePosition != tilePosition)
            {
                battleSystem.enemyGO.GetComponent<Settings>().highlightedTilePosition = tilePosition;
                groundTilemap.SetTile(battleSystem.enemyGO.GetComponent<Settings>().highlightedTilePosition, highlightedTile);
            }
        }
    }

    //Checks if there is a free tile in the space 
    int isThereAFreeTile(Func<float, float, float> operation, Vector3 tilePos, int maxTilesToMove)
    {
        int x = maxTilesToMove;
        while (x > 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(operation(tilePos.x, x), tilePos.y), Vector3.up, 30f, ~LayerMask.GetMask("Ground"));
               // Debug.DrawRay(new Vector2(operation(tilePos.x, x), tilePos.y), Vector3.up * 30f, Color.red, 10, true);
            if (hit.collider == null) return x;
                x--;
            }
        return x;
    }
       /* int EntityOnTile(Func<float, float, float> operation, Vector3 tilePos, int offset, bool isEnemyCloseToPlayer)
    {
        int x = 0;
        if (isEnemyCloseToPlayer)
        {
            Debug.Log("TRUE");
            while (x < numberofTilesToMoveOn)
            {
                Debug.Log("X < Current # of Tiles, so X: " + x);
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(operation(tilePos.x + 0.5f, x), tilePos.y), Vector3.up, 30f, ~LayerMask.GetMask("Ground"));
                Debug.DrawRay(new Vector2(operation(tilePos.x + 0.5f, x), tilePos.y), Vector3.up * 30f, Color.red, 10, true);
                if (hit.collider == null) return x;
                x++;
            } 
        } else
        {
            Debug.Log("FALSE");
            x = numberofTilesToMoveOn;
            while (x > 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(operation((tilePos.x), x), tilePos.y), Vector3.up, 30f, ~LayerMask.GetMask("Ground"));
                Debug.DrawRay(new Vector2(operation(tilePos.x, x), tilePos.y), Vector3.up * 30f, Color.red, 10, true);
                if (hit.collider == null)
                {
                    Debug.Log("NULL");
                    Debug.Log("NUMBER OF TILES TO MOVE: " + x);
                    return x;
                }
                x--;
            }
        }

        Debug.Log("X > Current # of Tiles, so X: " + x);
        return x;
    }*/

    override protected void MoveToTile()
    {
        //Debug.Log("Moving To Tile");
        Transform enemyTransform = battleSystem.enemyGO.transform;
        if (enemyTransform.position.x != battleSystem.enemyGO.GetComponent<Settings>().highlightedTilePosition.x + 0.5)
        {
            if (enemyTransform.position.x < battleSystem.enemyGO.GetComponent<Settings>().highlightedTilePosition.x + 0.5)
            {
                enemyTransform.Translate(Vector2.right * tileTravelSpeed * Time.deltaTime);
                if (enemyTransform.position.x >= battleSystem.enemyGO.GetComponent<Settings>().highlightedTilePosition.x + 0.5)
                {
                    MovingToTiles();
                    battleSystem.enemyGO.GetComponent<Settings>().canHighlightTile = true;
                }

            }
            else if (enemyTransform.position.x > battleSystem.enemyGO.GetComponent<Settings>().highlightedTilePosition.x + 0.5)
            {
                enemyTransform.Translate(-Vector2.right * tileTravelSpeed * Time.deltaTime);
                if (enemyTransform.position.x <= battleSystem.enemyGO.GetComponent<Settings>().highlightedTilePosition.x + 0.5)
                {
                    MovingToTiles();
                    battleSystem.enemyGO.GetComponent<Settings>().canHighlightTile = true;
                }

            }

        }

    }

    float Add(float a, float b)
    {
        return a + b;
    }

    float Subtract(float a, float b)
    {
        return a - b;
    }
}
