using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    private PlayerChoosingTiles playerChoosingTiles;
    private bool canFlip;
    [HideInInspector]
    public bool hasFireCollided;
    public float fireSpeed;

    private Transform fireTransform;
    // Start is called before the first frame update
    void Start()
    {
        canFlip = true;
        playerChoosingTiles = GameObject.Find("Grid").GetComponent<PlayerChoosingTiles>();
        fireTransform = GetComponent<Transform>();
        hasFireCollided = false;
    }

    void Flip()
    {
        canFlip = false;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    // Update is called once per frame
    void Update()
    {
        if (playerChoosingTiles.movementDirection == 1)
        {
            if (canFlip)
            Flip();
            fireTransform.Translate(-Vector2.right * fireSpeed * Time.deltaTime);
        } else if (playerChoosingTiles.movementDirection == -1)
        {
            fireTransform.Translate(-Vector2.right * fireSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasFireCollided = true;
    }

}
