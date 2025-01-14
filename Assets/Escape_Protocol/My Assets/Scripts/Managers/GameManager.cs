using GD.Items;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        InGame,
        Paused,
        GameOver,
        Victory,
        Settings
    }

    public static GameManager Instance { get; private set; }

    public GameState CurrentGameState { get; private set; }

    [SerializeField] private UIDocument mainMenuUI;
    [SerializeField] private UIDocument inGameUI;
    [SerializeField] private UIDocument settingsUI;
    [SerializeField] private UIDocument gameOverUI;
    [SerializeField] private UIDocument victoryUI;
    [SerializeField] private UIDocument pauseUI;
    [SerializeField] private SelectionManager selectionManager;

    public Inventory playerInventory;
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
        ShowMainMenu();
        if (playerInventory != null)
        {
            playerInventory.Clear();
        }
    }

    public void ShowMainMenu()
    {
        if (mainMenuUI != null)
        {
            mainMenuUI.gameObject.SetActive(true);
            inGameUI.gameObject.SetActive(false);
        }

        if (settingsUI != null)
        {
            settingsUI.gameObject.SetActive(false);
        }
    }


    public void StartGame()
    {
        CurrentGameState = GameState.InGame;

        mainMenuUI.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(true);


        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        if (selectionManager != null)
        {
            selectionManager.SetCrosshairVisibility(true);
        }
    }

    public void ShowSettings()
    {
        // Assuming settingsMenuUI is a reference to the UIDocument for the settings menu
        if (settingsUI != null)
        {
            settingsUI.gameObject.SetActive(true);
            // Optionally, deactivate other menus or pause the game if needed
            mainMenuUI.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Settings menu UI reference is missing in GameManager.");
        }
    }

    public void ShowPauseMenu()
    {
        if (pauseUI != null)
        {
            pauseUI.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ShowSettingsFromPause()
    {
        if (settingsUI != null)
        {
            settingsUI.gameObject.SetActive(true);

            // Optionally hide other menus
            if (pauseUI != null) pauseUI.gameObject.SetActive(false);
        }
    }

    public void ShowMainMenuFromPause()
    {
        if (mainMenuUI != null)
        {
            mainMenuUI.gameObject.SetActive(true);

            // Hide other menus and reset time scale
            if (pauseUI != null) pauseUI.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
