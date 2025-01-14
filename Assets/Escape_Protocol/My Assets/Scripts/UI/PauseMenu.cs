using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private UIDocument pauseMenuUI;
    private Button resumeButton;
    private Button settingsButton;
    private Button mainMenuButton;

    private bool isPaused = false;

    private void Start()
    {
        pauseMenuUI = GetComponent<UIDocument>();
        if (pauseMenuUI == null)
        {
            Debug.LogError("Pause menu UIDocument is not attached to this GameObject.");
            return;
        }

        var root = pauseMenuUI.rootVisualElement;

        // Query buttons
        resumeButton = root.Q<Button>("ResumeButton");
        settingsButton = root.Q<Button>("SettingsButton");
        mainMenuButton = root.Q<Button>("HomeButton");

        // Register button click events
        if (resumeButton != null) resumeButton.clicked += ResumeGame;
        if (settingsButton != null) settingsButton.clicked += OpenSettings;
        if (mainMenuButton != null) mainMenuButton.clicked += ReturnToMainMenu;

        // Ensure the pause menu is hidden initially
        pauseMenuUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Toggle key pressed!");

            if (isPaused)
            {
                Debug.Log("Resuming game...");
                ResumeGame();
            }
            else
            {
                Debug.Log("Pausing game...");
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.gameObject.SetActive(true);

            // Ensure the root element is visible
            var root = pauseMenuUI.rootVisualElement;
            root.style.display = DisplayStyle.Flex;
            root.style.opacity = 1;

            Debug.Log("Pause menu is now active.");
        }
        else
        {
            Debug.LogError("Pause menu UI reference is null!");
        }
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.gameObject.SetActive(false);
            Debug.Log("Pause menu is now inactive.");
        }
    }

    private void OpenSettings()
    {
        if (gameManager != null)
        {
            gameManager.ShowSettings();
        }
    }

    private void ReturnToMainMenu()
    {
        if (gameManager != null)
        {
            Time.timeScale = 1f;
            gameManager.ShowMainMenu();
        }
    }
}
