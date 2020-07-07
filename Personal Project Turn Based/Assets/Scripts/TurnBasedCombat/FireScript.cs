using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    private ChoosingTiles choosingTiles;
    private bool canFlip;
    [HideInInspector]
    public bool hasFireCollided;
    public float fireSpeed;

    private Transform fireTransform;
    // Start is called before the first frame update
    void Start()
    {
        canFlip = true;
        choosingTiles = GameObject.Find("Grid").GetComponent<ChoosingTiles>();
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
        if (choosingTiles.movementDirection == 1)
        {
            if (canFlip)
            Flip();
            fireTransform.Translate(-Vector2.right * fireSpeed * Time.deltaTime);
        } else if (choosingTiles.movementDirection == -1)
        {
            fireTransform.Translate(-Vector2.right * fireSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasFireCollided = true;
    }

}
