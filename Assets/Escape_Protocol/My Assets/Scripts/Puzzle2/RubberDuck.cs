using GD.Items;
using UnityEngine;

public class RubberDuck : MonoBehaviour
{
    public AudioClip collectSound; // Sound to play on collection
    public bool isCollected = false;
    public ItemData itemData;
    public void Collect()
    {
        if (isCollected) return;

        isCollected = true;

        // Play collect sound
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        // Notify the manager
        DuckCollectionManager.Instance.CollectDuck(this);

        // Hide the duck
        gameObject.SetActive(false);
    }
}
