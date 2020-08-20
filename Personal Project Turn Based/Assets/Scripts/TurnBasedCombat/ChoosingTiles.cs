using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class ChoosingTiles : MonoBehaviour
{
   // protected bool canHighlightTile;
    public bool isFacingOtherWay;
  //  public bool entityHasMovedToHighlightedTile;

    public int tileTravelSpeed;
    public int numberofTilesToMoveOn;
    [HideInInspector]
    public int movementDirection;


    [HideInInspector]
    

    protected Vector3 worldPoint;


    public Tilemap groundTilemap;
    public TileBase highlightedTile;
    public TileBase highlightedAttackTile;
    public TileBase nonHighlightedTile;
   // public ChoosingTiles opposingChoosingTiles;
    public BattleSystem battleSystem;
   // protected GameObject entity;

   // protected Transform entityTransform;
    protected GameObject currUnit;
    public int minNumOfUnitsThatCanSpawn;
    public int maxNumOfUnitsThatCanSpawn;
    [HideInInspector]
    public int numOfUnitsToSpawn;
    [HideInInspector]
    public List<GameObject> spawnedUnits = new List<GameObject>();


    //Highlights tile pointed to by mouse if it's not already highlighted anymore
    protected abstract void HighlightTile();

    //Entity moves to highlighted tile
    protected abstract void MoveToTile();

    // Start is called before the first frame update
    void Start()
    {
        currUnit.GetComponent<Settings>().entityHasMovedToHighlightedTile = true;
        isFacingOtherWay = false;
        movementDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Checks the movement direction of the entity. If entity is facing another way, flip it
    protected void CheckMovementDirection()
    {
        if (isFacingOtherWay)
            Flip();
    }

    //Flips all sprites
    protected void Flip()
    {
        if (!currUnit.GetComponent<Settings>().entityHasMovedToHighlightedTile)
        isFacingOtherWay = false;
        currUnit.GetComponent<Settings>().entityTransform.Rotate(0.0f, 180.0f, 0.0f);
        for (int i = 0; i < currUnit.GetComponent<Settings>().entity.transform.GetChildCount(); i++)
        {
            GameObject c = currUnit.GetComponent<Settings>().entity.transform.GetChild(i).gameObject;
            Vector3 cScale = c.transform.localScale;
            cScale.x *= -1;
            c.transform.localScale = cScale;
        }
    }


    //Makes a tile not highlighted anymore, if it's position is not equal to the position
    // of the highlightd tile
    protected void UnHighlightTile()
    {
        if (currUnit.GetComponent<Settings>().originalTilePosition != currUnit.GetComponent<Settings>().highlightedTilePosition)
        {
            groundTilemap.SetTile(currUnit.GetComponent<Settings>().originalTilePosition, nonHighlightedTile);
        }
    }

    //Highlights tile if highlighted tile position is not the same as the given tilePosition
    protected void HighlightTheTile(Vector3Int tilePosition)
    {
        if (currUnit.GetComponent<Settings>().highlightedTilePosition != tilePosition)
        {
            currUnit.GetComponent<Settings>().highlightedTilePosition = tilePosition;
            groundTilemap.SetTile(currUnit.GetComponent<Settings>().highlightedTilePosition, highlightedTile);
        }
    }

    //Resets everything once highlighted tile is reached
    protected void MovingToTiles()
    {
        currUnit.GetComponent<Settings>().entityTilePosition = new Vector3Int(currUnit.GetComponent<Settings>().highlightedTilePosition.x, currUnit.GetComponent<Settings>().highlightedTilePosition.y, currUnit.GetComponent<Settings>().highlightedTilePosition.z);
        UnHighlightTile();
        currUnit.GetComponent<Settings>().entityHasMovedToHighlightedTile = true;
    }
}
