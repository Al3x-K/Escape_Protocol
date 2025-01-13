using GD.Items;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Inventory playerInventory;

    private void Start()
    {
        if (playerInventory != null)
        {
            playerInventory.Clear();
        }
    }
}
