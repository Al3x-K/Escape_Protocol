using UnityEngine;

public class Interactable : MonoBehaviour
{
    public ItemData itemData; // Assign the ScriptableObject in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Picked up: {itemData.itemName}");
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.AddItem(itemData);
                Destroy(gameObject);
            }
        }
    }
}
