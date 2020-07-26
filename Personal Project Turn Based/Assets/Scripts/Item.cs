using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public enum Type
    {
        HEALING, BUFF, ATTACK
    }

    public string itemName;


    public float itemPower;

    public Type itemType;

    public Text itemText;

    public Sprite itemIcon;

    public Dictionary<string, int> itemStats = new Dictionary<string, int>();


    public Item(string itemName, Type itemType, Dictionary<string, int> itemStats)
    {
        this.itemName = itemName;
        this.itemType = itemType;
        this.itemText = itemText;
        this.itemStats = itemStats;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (itemName.Contains("Heal"))
        {
            itemType = Type.HEALING;
        } else if (itemName.Contains("Buff"))
        {
            itemType = Type.BUFF;
        } else if (itemName.Contains("Attack"))
        {
            itemType = Type.ATTACK;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override bool Equals(object other)
    {
        if (other == this) return true;
        if (other == null || GetType() != other.GetType()) return false;

        Item item = (Item) other;

        return item.itemName == this.itemName;
    }

    public override int GetHashCode()
    {
        return itemName.GetHashCode();
    }




}
