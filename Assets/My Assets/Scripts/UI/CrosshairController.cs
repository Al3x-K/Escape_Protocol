using UnityEngine;
using UnityEngine.UIElements;

public class CrosshairController : MonoBehaviour
{
    private VisualElement crosshair; // Crosshair UI element
    private Label interactPrompt;    // Interaction prompt (e.g., "Press E")
    private Camera mainCamera;

    public float interactDistance = 3f; // Max interaction distance

    private Transform currentSelection;
    private string selectableTag = "Selectable";

    private void Start()
    {
        // Cache the main camera
        mainCamera = Camera.main;

        // Set up UI Toolkit references
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
        // If there's a previous selection, reset its crosshair state
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

                // Show the interaction prompt
                interactPrompt.text = "Press E to interact";
                interactPrompt.style.display = DisplayStyle.Flex;

                // Optional: Debug log
                Debug.Log($"Aiming at {selection.name}");

                // Handle interaction (e.g., picking up items) when pressing "E"
                if (Input.GetKeyDown(KeyCode.E))
                {
                    HandleInteraction(selection);
                }

                return; // Exit early to avoid resetting unnecessarily
            }
        }
        
        // If no valid selection is found, hide the prompt
        interactPrompt.style.display = DisplayStyle.None;
    }

    private void ResetCrosshairState()
    {
        currentSelection = null;
        interactPrompt.style.display = DisplayStyle.None;
    }

    private void HandleInteraction(Transform selection)
    {
        // Attempt to get the Keycard component
        var keycard = selection.GetComponent<Keycard>();
        if (keycard != null)
        {
            Debug.Log($"Attempting to pick up keycard: {keycard.itemData.itemName}");

            // Attempt to get the PlayerInventory component
            PlayerInventory playerInventory = GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                // Add the item to the inventory
                playerInventory.AddItem(keycard.itemData);

                // Destroy the keycard object
                Destroy(keycard.gameObject);

                Debug.Log($"Picked up keycard: {keycard.itemData.itemName}");
            }
            else
            {
                Debug.LogWarning("PlayerInventory not found on the player.");
            }
        }
        else
        {
            Debug.LogWarning("Keycard component not found on the targeted object.");
        }
    }
}
