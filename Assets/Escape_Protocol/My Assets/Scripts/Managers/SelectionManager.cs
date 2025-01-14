using UnityEngine;
using UnityEngine.UIElements;
using GD.Items;
using static GameManager;
using GD.Types;
using GD.Audio;

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

    private GameManager gameManager;

    public AudioClip cardpPickupSound;
    public AudioClip duckPickupSound;
    public AudioMixerGroupName audioGroup = AudioMixerGroupName.Voiceover;


    private void Start()
    {
        // Cache the main camera
        mainCamera = Camera.main;

        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        crosshair = root.Q<VisualElement>("crosshair");
        interactPrompt = root.Q<Label>("InteractPrompt");

        if (interactPrompt == null)
        {
            Debug.LogError("InteractPrompt not found in the UI document.");
        }


        if (gameManager.CurrentGameState == GameManager.GameState.MainMenu)
        {
            crosshair.style.display = DisplayStyle.None;
            interactPrompt.style.display = DisplayStyle.None;
        }
        else
        {
            crosshair.style.display = DisplayStyle.Flex;
        }
       
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
                if (interactPrompt != null)
                {
                    interactPrompt.text = "Press E to interact";
                    interactPrompt.style.display = DisplayStyle.Flex;
                }
                else
                {
                    Debug.LogWarning("InteractPrompt is null and cannot be updated.");
                }

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
                PlaySound(cardpPickupSound);

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
        
        var rubberDuck = selection.GetComponent<RubberDuck>();
        if (rubberDuck != null && !rubberDuck.isCollected)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (playerInventory != null)
                {
                    playerInventory.Add(rubberDuck.itemData, 1); // Add the keycard to the inventory
                    PlaySound(duckPickupSound);

                    Destroy(rubberDuck.gameObject); // Remove the keycard from the scene

                    Debug.Log($"Picked up keycard: {rubberDuck.itemData.ItemType}");
                }
            }
        }


        Debug.LogWarning("The selected object does not have a valid interactable component.");
    }

    public void SetCrosshairVisibility(bool isVisible)
    {
        if (crosshair != null)
        {
            crosshair.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
            interactPrompt.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    private void PlaySound(AudioClip sound)
    {
        if (sound != null)
        {
            AudioManager.Instance.PlaySound(sound, audioGroup, transform.position);
        }
        else
        {
            Debug.LogWarning("Puzzle complete sound is not assigned.");
        }
    }
}
