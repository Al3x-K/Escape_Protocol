using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public string requiredKeyID;

    private bool isUnlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isUnlocked && other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null && inventory.HasItem(requiredKeyID))
            {
                Debug.Log("Door unlocked!");
                UnlockDoor();
            }
            else
            {
                Debug.Log("Door is locked. Missing required keycard.");
            }
        }
    }

    private void UnlockDoor()
    {
        isUnlocked = true;
        gameObject.SetActive(false); // Example: Deactivate the door.
    }
}
