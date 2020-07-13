using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerChoosingTiles : ChoosingTiles
{
    [HideInInspector]
    public bool mouseButtonPressed;
    [HideInInspector]
    public int movementDirection;

    private void Start()
    {
        entityTilePosition = new Vector3Int((int)-6.5, 0, 0);
        highlightedTilePosition = entityTilePosition;
        entityTransform = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform;
        entity = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition; //gets the position of the mouse
        Ray ray = Camera.main.ScreenPointToRay(mousePosition); //Points a ray from camera to mouse position
        worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);

        if (entityHasMovedToHighlightedTile)
            CheckFacingEnemy();
        else
        CheckMovementDirection();

        float mouseY = Mathf.Floor(worldPoint.y);
        float mouseX = Mathf.Floor(worldPoint.x);
        float enemyY = Mathf.Floor(opposingChoosingTiles.entityTilePosition.y);
        float enemyX = Mathf.Floor(opposingChoosingTiles.entityTilePosition.x);

        if (mouseX != enemyX && mouseY != enemyY + 1 && mouseY != enemyY + 2) {
            if (!isEntityMovingToHighlightedTile && !mouseButtonPressed)
            {
                UnHighlightTile();

                originalTilePostion = highlightedTilePosition; //makes the originalTilePosition = to the highlighted position

                if (Physics2D.Raycast(worldPoint, worldPoint))
                {
                    HighlightTile();
                }
            }

            if (Input.GetMouseButtonDown(0) && Physics2D.Raycast(worldPoint, worldPoint) && !battleSystem.hasPlayerAttacked) //left click is pressed
            {
                mouseButtonPressed = true;
                UnHighlightTile();
                originalTilePostion = highlightedTilePosition; //makes the originalTilePosition = to the highlighted position

                if (Physics2D.Raycast(worldPoint, worldPoint))
                {
                    HighlightTile();
                }
            }
        }

        if (!battleSystem.hasPlayerAttacked && !entityHasMovedToHighlightedTile)
        {
            isEntityMovingToHighlightedTile = true;
            MoveToTile();
        }
    }

    void CheckFacingEnemy()
    {
        float enemyPosition = Mathf.Round(opposingChoosingTiles.entityTilePosition.x);
        float playerPosition = Mathf.Round(entityTilePosition.x);
        if (Mathf.Round(Mathf.Abs(entityTilePosition.x - opposingChoosingTiles.entityTilePosition.x)) > 1 || playerPosition < enemyPosition && movementDirection == -1 || playerPosition > enemyPosition && movementDirection == 1)
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
        if (highlightedTilePosition != tilePosition)
        {
            highlightedTilePosition = tilePosition;
            groundTilemap.SetTile(highlightedTilePosition, highlightedTile);
        }
    }

    override protected void MoveToTile()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (isEntityMovingToHighlightedTile && playerTransform.position.x != highlightedTilePosition.x + 0.5)
        {
            if (playerTransform.position.x < highlightedTilePosition.x + 0.5)
            {
                if (movementDirection != 1)
                {
                    isFacingOtherWay = true;
                    movementDirection = 1;
                }
                playerTransform.Translate(Vector2.right * tileTravelSpeed* Time.deltaTime);
                if (playerTransform.position.x >= highlightedTilePosition.x + 0.5)
                {
                    MovingToTiles();
                }

            }
            else if (playerTransform.position.x > highlightedTilePosition.x + 0.5)
            {
                if (movementDirection != -1)
                {
                    isFacingOtherWay = true;
                    movementDirection = -1;
                }
                playerTransform.Translate(-Vector2.right * tileTravelSpeed * Time.deltaTime);
                if (playerTransform.position.x <= highlightedTilePosition.x + 0.5)
                {
                    MovingToTiles();
                }

            }
           
        }
       
    }


}
