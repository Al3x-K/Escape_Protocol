using UnityEngine;
using UnityEngine.UIElements;
using GD.Audio;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private Button playButton;
    private Button settingsButton;
    private Button quitButton;

    private void OnEnable()
    {
        // Re-register event listeners whenever the menu is enabled
        RegisterEventListeners();
    }

    private void OnDisable()
    {
        // Optionally, unregister event listeners to prevent duplicate registration
        UnregisterEventListeners();
    }

    private void RegisterEventListeners()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        playButton = root.Q<Button>("PlayButton");
        settingsButton = root.Q<Button>("SettingsButton");
        quitButton = root.Q<Button>("QuitButton");

        if (playButton != null)
            playButton.clicked += OnPlayButtonClicked;

        if (settingsButton != null)
            settingsButton.clicked += OnSettingsButtonClicked;

        if (quitButton != null)
            quitButton.clicked += OnQuitButtonClicked;
    }

    private void UnregisterEventListeners()
    {
        if (playButton != null)
            playButton.clicked -= OnPlayButtonClicked;

        if (settingsButton != null)
            settingsButton.clicked -= OnSettingsButtonClicked;

        if (quitButton != null)
            quitButton.clicked -= OnQuitButtonClicked;
    }

    private void OnPlayButtonClicked()
    {
        gameManager.StartGame();
    }

    private void OnSettingsButtonClicked()
    {
        gameManager.ShowSettings();
    }

    private void OnQuitButtonClicked()
    {
        gameManager.QuitGame();
    }


}
