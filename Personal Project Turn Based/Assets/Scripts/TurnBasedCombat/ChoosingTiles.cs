using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class ChoosingTiles : MonoBehaviour
{
    public bool isEntityMovingToHighlightedTile;
    protected bool canHighlightTile;
    public bool isFacingOtherWay;
    public bool entityHasMovedToHighlightedTile;

    public int tileTravelSpeed;
    public int numberofTilesToMoveOn;
    [HideInInspector]
    public int movementDirection;

    [HideInInspector]
    public Vector3Int entityTilePosition;
    [HideInInspector]
    public Vector3Int highlightedTilePosition;
    protected Vector3Int originalTilePostion;

    protected Vector3 worldPoint;


    public Tilemap groundTilemap;
    public TileBase highlightedTile;
    public TileBase nonHighlightedTile;
    public ChoosingTiles opposingChoosingTiles;
    public BattleSystem battleSystem;
    protected GameObject entity;

    protected Transform entityTransform;


    //Highlights tile pointed to by mouse if it's not already highlighted anymore
    protected abstract void HighlightTile();

    //Entity moves to highlighted tile
    protected abstract void MoveToTile();

    // Start is called before the first frame update
    void Start()
    {
        entityHasMovedToHighlightedTile = true;
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
        if (!entityHasMovedToHighlightedTile)
        isFacingOtherWay = false;
        entityTransform.Rotate(0.0f, 180.0f, 0.0f);
        for (int i = 0; i < entity.transform.GetChildCount(); i++)
        {
            GameObject c = entity.transform.GetChild(i).gameObject;
            Vector3 cScale = c.transform.localScale;
            cScale.x *= -1;
            c.transform.localScale = cScale;
/*            for (int j = 0; j < c.transform.GetChildCount(); j++)
            {
                GameObject c2 = c.transform.GetChild(j).gameObject;
                Debug.Log(c2.gameObject.name);
                Vector3 c2Scale = c2.transform.localScale;
                c2Scale.x *= -1;
                c2.transform.localScale = c2Scale;
            }*/
        }
    }


    //Makes a tile not highlighted anymore, if it's position is not equal to the position
    // of the highlightd tile
    protected void UnHighlightTile()
    {
        if (originalTilePostion != highlightedTilePosition)
        {
            groundTilemap.SetTile(originalTilePostion, nonHighlightedTile);
        }
    }

    //Highlights tile if highlighted tile position is not the same as the given tilePosition
    protected void HighlightTheTile(Vector3Int tilePosition)
    {
        if (highlightedTilePosition != tilePosition)
        {
            highlightedTilePosition = tilePosition;
            groundTilemap.SetTile(highlightedTilePosition, highlightedTile);
        }
    }

    //Resets everything once highlighted tile is reached
    protected void MovingToTiles()
    {
        entityTilePosition = new Vector3Int(highlightedTilePosition.x, highlightedTilePosition.y, highlightedTilePosition.z);
        UnHighlightTile();
        isEntityMovingToHighlightedTile = false;
        entityHasMovedToHighlightedTile = true;
    }
}
