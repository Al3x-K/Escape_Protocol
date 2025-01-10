using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName; // Name of the item
    public string description; // Description of the item
    public Sprite icon; // Icon for UI
    public string keyID; // Unique identifier for keycards or other items
}
