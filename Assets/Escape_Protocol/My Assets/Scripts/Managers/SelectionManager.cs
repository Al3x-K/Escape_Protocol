using UnityEngine;
using UnityEngine.UIElements;
using GD.Items;
using System.Linq;

public class SelectionManager : MonoBehaviour
{
    private VisualElement crosshair; // Crosshair UI element
    private Label interactPrompt;    // Interaction prompt (e.g., "Press E")
    private Camera mainCamera;

    public float interactDistance = 3f; // Max interaction distance
    public Inventory playerInventory;   // Reference to the player's Inventory ScriptableObject

    private Transform currentSelection;
    private string selectableTag = "Selectable";

    private Keycard highlightedKeycard;
    private TerminalInteraction highlightedItem;

    private Transform heldCube;


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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldCube == null)
            {
                TryPickupCube();
            }
            else
            {
                TryPlaceCube();
            }
        }
    }
    private void TryPickupCube()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactDistance))
        {
            var cube = hitInfo.transform.GetComponent<PuzzleCube>();
            if (cube != null)
            {
                heldCube = cube.transform;
                heldCube.SetParent(mainCamera.transform); // Attach to camera
                heldCube.localPosition = new Vector3(0, -0.5f, 1f); // Position in front of the camera
                heldCube.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
            }
        }
    }

    private void TryPlaceCube()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactDistance))
        {
            var pedestal = hitInfo.transform.GetComponent<Pedestal>();
            if (pedestal != null)
            {
                heldCube.SetParent(null); // Detach from the camera
                heldCube.position = pedestal.transform.position; // Snap to pedestal
                heldCube.GetComponent<Rigidbody>().isKinematic = false; // Re-enable physics

                var cubeScript = heldCube.GetComponent<PuzzleCube>();
                if (cubeScript != null)
                {
                    cubeScript.CheckPlacement(pedestal.transform);
                }

                heldCube = null;
                return;
            }
        }

        // If not placing on a pedestal, drop the cube at the current position
        heldCube.SetParent(null);
        heldCube.GetComponent<Rigidbody>().isKinematic = false; // Re-enable physics
        heldCube = null;
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

    private void HighLightTerminal(TerminalInteraction terminal)
    {
        if (highlightedItem != null && highlightedItem != terminal)
        {
            highlightedItem.Highlight(false);
        }
        terminal.Highlight(true);
        highlightedItem = terminal;
    }

    private void HandleInteraction(Transform selection)
    {
        var keycard = selection.GetComponent<Keycard>();
        if (keycard != null)
        {
            Debug.Log($"Attempting to pick up keycard: {keycard.itemData.ItemType}");

            if (playerInventory != null)
            {
                playerInventory.Add(keycard.itemData, 1); // Add the keycard to the inventory

                Destroy(keycard.gameObject); // Remove the keycard from the scene

                Debug.Log($"Picked up keycard: {keycard.itemData.ItemType}");
            }
            else
            {
                Debug.LogWarning("Player inventory reference is missing.");
            }

            return;
        }

        var lockedDoor = selection.GetComponent<LockedDoor>();
        if (lockedDoor != null)
        {
            Debug.Log("Attempting to unlock door.");

            if (playerInventory != null && playerInventory.Contains(lockedDoor.requiredKeyItem))
            {
                Debug.Log("Door unlocked!");
                lockedDoor.PlayGrantAccessSound();
                lockedDoor.UnlockDoor();
            }
            else
            {
                Debug.Log("Door is locked. Missing required keycard.");
                lockedDoor.PlayDenySound();
            }

            return;
        }

        
        var terminal = selection.GetComponent<TerminalInteraction>();
        PuzzleDoor puzzleDoor = null;
        if (terminal != null)
        {
            Debug.Log("Interacting with terminal.");
            terminal.Interact();
            puzzleDoor.ToggleActiveState();
            return;
        }


        //Debug.LogWarning("The selected object does not have a valid interactable component.");
    }
}
