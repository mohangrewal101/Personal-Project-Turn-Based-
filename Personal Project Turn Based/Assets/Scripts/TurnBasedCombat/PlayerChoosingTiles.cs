using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerChoosingTiles : ChoosingTiles
{
    private bool isAttackTileHighlighted;
    [HideInInspector]
    public bool mouseButtonPressed;
    [HideInInspector]
    public int movementDirection;

    private void Start()
    {
        currUnit = battleSystem.playerGO;
        // entityTilePosition = currUnit.GetComponent<Settings>().entityTilePosition;
        battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition = currUnit.GetComponent<Settings>().entityTilePosition;
      //  entityTransform = currUnit.GetComponent<Settings>().entityTransform;
      //  entity = currUnit.GetComponent<Settings>().entity;
    }

    // Update is called once per frame
    void Update()
    {
        if (currUnit != battleSystem.playerGO)
        {
            currUnit = battleSystem.playerGO;
           // entityTilePosition = currUnit.GetComponent<Settings>().entityTilePosition;
          //  entityTransform = currUnit.GetComponent<Settings>().entityTransform;
            battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition = currUnit.GetComponent<Settings>().entityTilePosition;
          //  entity = currUnit.GetComponent<Settings>().entity;
        }
        Vector3 mousePosition = Input.mousePosition; //gets the position of the mouse
        Ray ray = Camera.main.ScreenPointToRay(mousePosition); //Points a ray from camera to mouse position
        worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);

        if (currUnit.GetComponent<Settings>().entityHasMovedToHighlightedTile)
            CheckFacingEnemy();
        else
        CheckMovementDirection();

        float mouseY = Mathf.Floor(worldPoint.y);
       // float mouseX = Mathf.Floor(worldPoint.x);
        float enemyY = Mathf.Floor(battleSystem.enemyGO.GetComponent<Settings>().entityTilePosition.y);
       // float enemyX = Mathf.Floor(opposingChoosingTiles.entityTilePosition.x);

        if (/*mouseX != enemyX &&*/ mouseY != enemyY + 1 && mouseY != enemyY + 2) {
            if (currUnit.GetComponent<Settings>().entityHasMovedToHighlightedTile && !mouseButtonPressed)
            {
                UnHighlightTile();

                battleSystem.playerGO.GetComponent<Settings>().originalTilePosition = battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition; //makes the originalTilePosition = to the highlighted position

                if (Physics2D.Raycast(worldPoint, worldPoint))
                {
                    HighlightTile();
                }
            }

            if (Input.GetMouseButtonDown(0) && Physics2D.Raycast(worldPoint, worldPoint) && !currUnit.GetComponent<Settings>().hasAttacked) //left click is pressed
            {
                mouseButtonPressed = true;
                UnHighlightTile();
                battleSystem.playerGO.GetComponent<Settings>().originalTilePosition = battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition; //makes the originalTilePosition = to the highlighted position

                if (Physics2D.Raycast(worldPoint, worldPoint))
                {
                    HighlightTile();
                }
            }
        }

        if (!currUnit.GetComponent<Settings>().hasAttacked && !currUnit.GetComponent<Settings>().entityHasMovedToHighlightedTile)
        {
            MoveToTile();
        }
    }

    void CheckFacingEnemy()
    {
        float enemyPosition = Mathf.Round(battleSystem.enemyGO.GetComponent<Settings>().entityTilePosition.x);
        float playerPosition = Mathf.Round(currUnit.GetComponent<Settings>().entityTilePosition.x);
        if (Mathf.Round(Mathf.Abs(currUnit.GetComponent<Settings>().entityTilePosition.x - battleSystem.enemyGO.GetComponent<Settings>().entityTilePosition.x)) > 1 || playerPosition < enemyPosition && movementDirection == -1 || playerPosition > enemyPosition && movementDirection == 1)
        {
            if (playerPosition < enemyPosition && movementDirection == -1)
            {
                movementDirection = 1;
                Flip();
            }
            else if (playerPosition > enemyPosition && movementDirection == 1)
            {
                movementDirection = -1;
                Flip();
            }
        }
    }

    override protected void HighlightTile()
    {

        Vector3Int tilePosition = groundTilemap.WorldToCell(worldPoint);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(tilePosition.x + 0.5f, tilePosition.y), Vector3.up, 30f, ~LayerMask.GetMask("Ground"));
        if (battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition != tilePosition)
        {
            battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition = tilePosition;
            if (hit.collider == null || hit.collider.tag == "Player")
            {
                isAttackTileHighlighted = false;
                groundTilemap.SetTile(battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition, highlightedTile);
            } else
            {
                isAttackTileHighlighted = true;
                groundTilemap.SetTile(battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition, highlightedAttackTile);
            }
        }
    }

    override protected void MoveToTile()
    {
        //  Transform playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        float posToTravelTo = 0;
        if (isAttackTileHighlighted)
        {
            Debug.Log("ATTACK TILE");
            posToTravelTo = battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition.x + 1.5f;
        }
        else posToTravelTo = battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition.x + 0.5f;
        Transform playerTransform = currUnit.transform;
        if (playerTransform.position.x != posToTravelTo)
        {
            if (playerTransform.position.x < posToTravelTo)
            {
                if (movementDirection != 1)
                {
                    isFacingOtherWay = true;
                    movementDirection = 1;
                }
                playerTransform.Translate(Vector2.right * tileTravelSpeed* Time.deltaTime);
                if (playerTransform.position.x >= posToTravelTo)
                {
                    MovingToTiles();
                    if (isAttackTileHighlighted)
                        currUnit.GetComponent<Settings>().entityTilePosition = new Vector3Int(battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition.x + 1, battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition.y, battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition.z);
                }

            }
            else if (playerTransform.position.x > posToTravelTo)
            {
                if (movementDirection != -1)
                {
                    isFacingOtherWay = true;
                    movementDirection = -1;
                }
                playerTransform.Translate(-Vector2.right * tileTravelSpeed * Time.deltaTime);
                if (playerTransform.position.x <= posToTravelTo)
                {
                    MovingToTiles();
                    if (isAttackTileHighlighted)
                        currUnit.GetComponent<Settings>().entityTilePosition = new Vector3Int(battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition.x + 1, battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition.y, battleSystem.playerGO.GetComponent<Settings>().highlightedTilePosition.z);
                }

            }
           
        }
       
    }


}
