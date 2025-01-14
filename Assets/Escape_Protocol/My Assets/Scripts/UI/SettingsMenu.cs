using GD.Audio;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private Slider masterVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;
    private Button backButton;

    private void Start()
    {
        // Get the UIDocument and its root
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Query UI elements
        musicVolumeSlider = root.Q<Slider>("MusicSlider");
        backButton = root.Q<Button>("Quit");

        // Load saved settings
        LoadSettings();

        // Register events
        musicVolumeSlider.RegisterValueChangedCallback(evt => OnVolumeChanged(evt.newValue, "MusicVolume"));

        backButton.clicked += OnBackButtonClicked;
    }

    private void LoadSettings()
    {
        // Load settings from PlayerPrefs
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
    }

    private void SaveSettings()
    {
        // Save settings to PlayerPrefs
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.Save();
    }

    private void OnVolumeChanged(float value, string settingName)
    {
        Debug.Log($"{settingName} set to {value}");
    }

    private void OnBackButtonClicked()
    {
        SaveSettings();
        gameManager.ShowMainMenu();
    }
}
