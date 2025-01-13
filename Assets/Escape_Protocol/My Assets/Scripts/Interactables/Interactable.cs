using UnityEngine;
using GD.Items;

public class Interactable : MonoBehaviour
{
    public ItemData itemData; // Assign the ScriptableObject in the Inspector
    public Inventory playerInventory; // Reference to the Inventory ScriptableObject

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemData == null)
            {
                Debug.LogError("ItemData is not assigned to this Interactable.");
                return;
            }

            Debug.Log($"Picked up: {itemData.itemName}");

            if (playerInventory != null)
            {
                // Add the item to the inventory
                playerInventory.Add(itemData, 1);

                // Destroy the interactable object after picking it up
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Player Inventory ScriptableObject is not assigned.");
            }
        }
    }
}
