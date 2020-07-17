using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private UnitScript playerUnit;
    public List<Item> inventoryItems;
    public Item healingItem;
    public Dictionary<Item, float> itemNumbers = new Dictionary<Item, float>();
    private Dictionary<string, float> itemStats = new Dictionary<string, float>();

    public Sprite tempItemSprite;
    // Start is called before the first frame update
    void Start()
    {
        inventoryItems = new List<Item>();
        playerUnit = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitScript>();
/*        Item itemUnit = GameObject.FindGameObjectWithTag("Item").GetComponent<Item>();
        addItem(itemUnit);*/
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //MODIFIES: this
    //EFFECTS: Adds given item to list of items
    public void addItem(Item item)
    {
        if (!inventoryItems.Contains(item))
        {
            inventoryItems.Add(item);
            Debug.Log("ITEM ADDED");
            itemNumbers.Add(item, 1);
            itemStats.Add(item.itemName, item.itemPower);
        }
        //INCREASE ITEM NUMBER
        itemNumbers[item] = itemNumbers[item] + 1;
    }

    //MODIFIES: this
    //EFFECTS: Decreases the item number from list of items
    public void removeItem(Item item)
    {
        //DECREASE ITEM NUMBER
        if (inventoryItems.Contains(item))
        {
            itemNumbers[item] = itemNumbers[item] - 1;

            if (itemNumbers[item] <= 0)
            {
                inventoryItems.Remove(item);
                itemNumbers.Remove(item);
                itemStats.Remove(item.itemName);
            }
        }
    }


    //EFFECTS: Ues the item in player's inventory
    public void useItem(Item i)
    {
        switch (i.itemType)
        {
            case Item.Type.HEALING:
                playerUnit.Heal(i.itemPower);
                break;
            case Item.Type.BUFF:

                break;
            default:

                break;
        }
        removeItem(i);
    }
}
