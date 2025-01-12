using UnityEngine;
using UnityEngine.UIElements;

public class SelectionManager : MonoBehaviour
{
    private VisualElement crosshair; // Crosshair UI element
    private Label interactPrompt;    // Interaction prompt (e.g., "Press E")
    private Camera mainCamera;

    public float interactDistance = 3f; // Max interaction distance

    private Transform currentSelection;
    private string selectableTag = "Selectable";

    private Keycard highlightedKeycard;

    private void Start()
    {
        // Cache the main camera
        mainCamera = Camera.main;

        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        crosshair = root.Q<VisualElement>("Crosshair");
        interactPrompt = root.Q<Label>("InteractPrompt");

        // Hide the interaction prompt by default
        interactPrompt.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        UpdateCrosshairState();
    }

    private void UpdateCrosshairState()
    {
        if (currentSelection != null)
        {
            ResetCrosshairState();
        }
        
        // Raycast from the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactDistance))
        {
            var selection = hitInfo.transform;

            // Check if the hit object has the "Selectable" tag
            if (selection.CompareTag(selectableTag))
            {
                currentSelection = selection;

                var keycard = selection.GetComponent<Keycard>();
                if (keycard != null)
                {
                    HighlightKeycard(keycard);
                }

                // Show the interaction prompt
                interactPrompt.text = "Press E to interact";
                interactPrompt.style.display = DisplayStyle.Flex;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    HandleInteraction(selection);
                }

                return; 
            }
        }
        
        interactPrompt.style.display = DisplayStyle.None;
    }

    private void ResetCrosshairState()
    {
        // Reset current selection
        currentSelection = null;

        // Remove highlight from the previously highlighted keycard
        if (highlightedKeycard != null)
        {
            highlightedKeycard.Highlight(false);
            highlightedKeycard = null;
        }

        interactPrompt.style.display = DisplayStyle.None;
    }

    private void HighlightKeycard(Keycard keycard)
    {
        if (highlightedKeycard != null && highlightedKeycard != keycard)
        {
            highlightedKeycard.Highlight(false);
        }

        keycard.Highlight(true);
        highlightedKeycard = keycard;
    }

    private void HandleInteraction(Transform selection)
    {
        var keycard = selection.GetComponent<Keycard>();
        if (keycard != null)
        {
            Debug.Log($"Attempting to pick up keycard: {keycard.itemData.itemName}");

            PlayerInventory playerInventory = GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.AddItem(keycard.itemData);

                Destroy(keycard.gameObject);

                Debug.Log($"Picked up keycard: {keycard.itemData.itemName}");
            }
            else
            {
                Debug.LogWarning("PlayerInventory not found on the player.");
            }

            return; 
        }

        var lockedDoor = selection.GetComponent<LockedDoor>();
        if (lockedDoor != null)
        {
            Debug.Log("Attempting to unlock door.");

            PlayerInventory playerInventory = GetComponent<PlayerInventory>();
            if (playerInventory != null && playerInventory.HasItem(lockedDoor.requiredKeyID))
            {
                Debug.Log("Door unlocked!");
                lockedDoor.UnlockDoor();
            }
            else
            {
                Debug.Log("Door is locked. Missing required keycard.");
            }

            return; 
        }

        Debug.LogWarning("The selected object does not have a valid interactable component.");
    }

}
