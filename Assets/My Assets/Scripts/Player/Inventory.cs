using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<ItemData> items = new List<ItemData>();

    public void AddItem(ItemData item)
    {
        items.Add(item);
        Debug.Log($"Added {item.itemName} to inventory.");
    }

    public bool HasItem(string keyID)
    {
        return items.Exists(item => item.keyID == keyID);
    }
}
