using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private VisualElement crosshair;
    private Label interactPrompt;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found on UIManager.");
            return;
        }

        var root = uiDocument.rootVisualElement;
        crosshair = root.Q<VisualElement>("crosshair");
        interactPrompt = root.Q<Label>("InteractPrompt");

        if (crosshair == null || interactPrompt == null)
        {
            Debug.LogError("Crosshair or InteractPrompt not found in the UI document.");
            return;
        }

        interactPrompt.style.display = DisplayStyle.None;
    }

    public void ShowInteractPrompt(string text)
    {
        if (interactPrompt != null)
        {
            interactPrompt.text = text;
            interactPrompt.style.display = DisplayStyle.Flex;
        }
    }

    public void HideInteractPrompt()
    {
        if (interactPrompt != null)
        {
            interactPrompt.style.display = DisplayStyle.None;
        }
    }
}
