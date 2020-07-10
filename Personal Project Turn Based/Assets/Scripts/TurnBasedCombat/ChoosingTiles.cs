using UnityEngine;
using UnityEngine.Tilemaps;

public class ChoosingTiles : MonoBehaviour
{
    private Transform playerTransform;
    private bool isplayerMovingToHighlightedTile;
    [HideInInspector]
    public bool mouseButtonPressed;
    [HideInInspector]
    public bool playerHasMovedToHighlightedTile;

    private bool isFacingWrongWay;
    [HideInInspector]
    public int movementDirection;
    public int tileTravelSpeed;

    [HideInInspector]
    public Vector3Int highlightedTilePosition;
    [HideInInspector]
    public Vector3Int originalTilePostion;

    [HideInInspector]
    public Vector3Int playerTilePosition;

    

    private Vector3 worldPoint;

    public Tilemap groundTilemap;
    public TileBase highlightedTile;
    public TileBase nonhighlightedTile;
    public BattleSystem battleSystem;
    public EnemyChoosingTiles enemyChoosingTiles;

    private void Start()
    {
        playerHasMovedToHighlightedTile = true;
        playerTilePosition = new Vector3Int((int)-6.5, 0, 0);
        playerTransform = GameObject.Find("PlayerBattleStation").transform.GetChild(0).GetChild(0).transform;
        isFacingWrongWay = false;
        movementDirection = 1;
    }
    // Update is called once per frame


    void CheckMovementDirection()
    {
        if (isFacingWrongWay)
            Flip();
    }

    void CheckFacingEnemy()
    {
        float enemyPosition = Mathf.Round(enemyChoosingTiles.enemyTilePosition.x);
        float playerPosition = Mathf.Round(playerTilePosition.x);
        if (Mathf.Round(Mathf.Abs(playerTilePosition.x - enemyChoosingTiles.enemyTilePosition.x)) > 1 || playerPosition < enemyPosition && movementDirection == -1 || playerPosition > enemyPosition && movementDirection == 1)
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

    void Flip()
    {
        if (!playerHasMovedToHighlightedTile)
        isFacingWrongWay = false;
        playerTransform.Rotate(0.0f, 180.0f, 0.0f);
        GameObject child = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        GameObject child2 = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        GameObject child3 = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(1).gameObject;

        Vector3 childScale = child.transform.localScale; //switches back UI
        childScale.x *= -1;
        child.transform.localScale = childScale;

        Vector3 childScale2 = child2.transform.localScale; //switches back UI
        childScale2.x *= -1;
        child2.transform.localScale = childScale2;

        Vector3 childScale3 = child3.transform.localScale; //switches back UI
        childScale3.x *= -1;
        child3.transform.localScale = childScale3;
    }
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition; //gets the position of the mouse
        Ray ray = Camera.main.ScreenPointToRay(mousePosition); //Points a ray from camera to mouse position
        worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);

        if (playerHasMovedToHighlightedTile)
            CheckFacingEnemy();
        else
        CheckMovementDirection();
        float mouseY = Mathf.Floor(worldPoint.y);
        float mouseX = Mathf.Floor(worldPoint.x);
        float enemyY = Mathf.Floor(enemyChoosingTiles.enemyTilePosition.y);
        float enemyX = Mathf.Floor(enemyChoosingTiles.enemyTilePosition.x);
        if (mouseX != enemyX && mouseY != enemyY + 1 && mouseY != enemyY + 2) {
            if (!isplayerMovingToHighlightedTile && !mouseButtonPressed)
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

        if (!battleSystem.hasPlayerAttacked && !playerHasMovedToHighlightedTile)
        {
            isplayerMovingToHighlightedTile = true;
            MoveToTile();
        }
    }

    //REQUIRES: tile to be highlighted
    //MODIFIES: this
    //EFFECTS: Makes a tile not highlighted anymore, if it's position is not equal to the position
    // of the highlightd tile
    void UnHighlightTile()
    {
        if (originalTilePostion != highlightedTilePosition)
        {
            groundTilemap.SetTile(originalTilePostion, nonhighlightedTile);
        }
    }

    //REQUIRES: tile to not be highlighted
    //MODIFIES: this
    //EFFECTS: Highlights tile if it's not already highlighted anymore
    void HighlightTile()
    {
       
        Vector3Int tilePosition = groundTilemap.WorldToCell(worldPoint);
        if (highlightedTilePosition != tilePosition)
        {
            highlightedTilePosition = tilePosition;
            Debug.Log("First" + groundTilemap.size);
            groundTilemap.SetTile(highlightedTilePosition, highlightedTile);
            Debug.Log("Second" + groundTilemap.size);
        }
    }

    //REQUIRES: Player is attacking
    //MODIFES: this
    //EFFECTS: Player moves to highlighted tile if mouse button is clicked
    public void MoveToTile()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (isplayerMovingToHighlightedTile && playerTransform.position.x != highlightedTilePosition.x + 0.5)
        {
            if (playerTransform.position.x < highlightedTilePosition.x + 0.5)
            {
                if (movementDirection != 1)
                {
                    isFacingWrongWay = true;
                    movementDirection = 1;
                }
                playerTransform.Translate(Vector2.right * tileTravelSpeed* Time.deltaTime);
                if (playerTransform.position.x >= highlightedTilePosition.x + 0.5)
                {
                    playerTilePosition = new Vector3Int(highlightedTilePosition.x, highlightedTilePosition.y, highlightedTilePosition.z);
                    
                    UnHighlightTile();
                    isplayerMovingToHighlightedTile = false;
                    playerHasMovedToHighlightedTile = true;
                }

            }
            else if (playerTransform.position.x > highlightedTilePosition.x + 0.5)
            {
                if (movementDirection != -1)
                {
                    isFacingWrongWay = true;
                    movementDirection = -1;
                }
                playerTransform.Translate(-Vector2.right * tileTravelSpeed * Time.deltaTime);
                if (playerTransform.position.x <= highlightedTilePosition.x + 0.5)
                {
                    playerTilePosition = new Vector3Int(highlightedTilePosition.x, highlightedTilePosition.y, highlightedTilePosition.z);
                    UnHighlightTile();
                    isplayerMovingToHighlightedTile = false;
                    playerHasMovedToHighlightedTile = true;
                }

            }
           
        }
       
    }


}
