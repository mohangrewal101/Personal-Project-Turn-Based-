using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentGeneration : MonoBehaviour
{
    public GameObject[] trees;

    private float rangeX;
    private int layerColorDiff; //the difference when objects change colour as they go in lower layer
    public int maxRange;
    public int maxOrderofLayerToGoOn;
    public int minOrderofLayerToGoOn; //layer for trees to go on

    private Transform environmentTransform;
    // Start is called before the first frame update
    void Start()
    {
        environmentTransform = gameObject.transform;
        layerColorDiff = 255 / (maxOrderofLayerToGoOn - minOrderofLayerToGoOn);
    }

    // Update is called once per frame
    void Update()
    {
        if (trees.Length != 0 && rangeX < maxRange)
        {
            
            GameObject currTree = Instantiate(trees[Random.Range(0, trees.Length - 1)], environmentTransform);

            int currLayer = Random.Range(minOrderofLayerToGoOn, maxOrderofLayerToGoOn);

            currTree.transform.position = new Vector3(environmentTransform.position.x + rangeX, environmentTransform.position.y, environmentTransform.position.z);

            SpriteRenderer treeRend = currTree.GetComponent<SpriteRenderer>();

            treeRend.sortingOrder = currLayer;

            float calculatedColor = 255 - (layerColorDiff * (maxOrderofLayerToGoOn - currLayer));
            treeRend.color = new Color(calculatedColor / 255, calculatedColor / 255, calculatedColor / 255);

            rangeX += 1.25f;
            
        }
    }
}
