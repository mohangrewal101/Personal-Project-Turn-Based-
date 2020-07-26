using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private UnitScript playerUnit;
    public List<Item> inventoryItems;
    public Dictionary<Item, float> itemNumbers = new Dictionary<Item, float>();
   // private Dictionary<string, float> itemStats = new Dictionary<string, float>();

    public Sprite tempItemSprite;
    // Start is called before the first frame update
    void Start()
    {
        inventoryItems = new List<Item>();
        playerUnit = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitScript>();
        /*        Item itemUnit = GameObject.FindGameObjectWithTag("Item").GetComponent<Item>();
                addItem(itemUnit);*/

        //NOTE: Build Inventory is temporary and will stay until I start working on out of battle gameplay again
        BuildInventory();
        
    }

    void BuildInventory()
    {
        Item healingItem = new Item("Healing Potion", Item.Type.HEALING, new Dictionary<string, int>()
        {
            {"Recover", 5}
        });

        Item buffItem = new Item("Defense Buff", Item.Type.BUFF, new Dictionary<string, int>()
        {
            {"DefenseUp", 10}
        });

        Item attackItem = new Item("Grenade", Item.Type.ATTACK, new Dictionary<string, int>()
        {
            {"Attack", 10}
        });
        addItem(healingItem);
        addItem(healingItem);
        //Ignore below two lines of code for now
       // addItem(buffItem);
       // addItem(attackItem);
    }

    //MODIFIES: this
    //EFFECTS: Adds given item to list of items
    public void addItem(Item item)
    {
        if (!inventoryItems.Contains(item))
        {
            inventoryItems.Add(item);
            itemNumbers.Add(item, 1);
          //  itemStats.Add(item.itemName, item.itemPower);
        } else itemNumbers[item] = itemNumbers[item] + 1;
    }

    //MODIFIES: this
    //EFFECTS: Decreases the item number from list of items
    public void removeItem(Item item)
    {
        //DECREASE ITEM NUMBER
        if (inventoryItems.Contains(item))
        {
            itemNumbers[item] = itemNumbers[item] - 1;
            Debug.Log("InventoryDecreased: " + itemNumbers[item]);

            if (itemNumbers[item] <= 0)
            {
                Debug.Log("NotInInventoryAnymore");
                inventoryItems.Remove(item);
                itemNumbers.Remove(item);
               // itemStats.Remove(item.itemName);
            }
        }
    }


/*    //EFFECTS: Ues the item in player's inventory
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

    }*/
}
