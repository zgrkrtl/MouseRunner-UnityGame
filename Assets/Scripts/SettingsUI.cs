using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SettingsUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button closeSettingsButton;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI masterVolumeNumberText;
    [SerializeField] private TextMeshProUGUI musicVolumeNumberText;
    [SerializeField] private TextMeshProUGUI sfxVolumeNumberText;
    [SerializeField] private Button muteButton;
    [SerializeField] private MainMenuAudioManager mainMenuMusic;

    [SerializeField] private TMP_Dropdown graphicsDropdown; 
    private void Start()
    {
        GameOverAndPauseUIController.OnMainMenuOpened +=GameOverAndPauseUIController_OnMainMenuOpened;
       
        int graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 2); // Default is medium
        graphicsDropdown.value = graphicsQuality;
        
        // Handle Canvases
        settingsButton.onClick.AddListener(OpenSettingsMenu);
        closeSettingsButton.onClick.AddListener(CloseSettingsMenu);
        muteButton.onClick.AddListener(MuteButtonClicked);
        // Handle Volume
        
        var savedVolume = PlayerPrefs.GetFloat("MasterVolume");
        masterVolumeSlider.value = savedVolume;
        SetMasterVolume(savedVolume);
        
        var savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        musicVolumeSlider.value = savedMusicVolume;
        SetMusicVolume(savedMusicVolume);
        mainMenuMusic.SetVolume(savedMusicVolume);

        sfxVolumeSlider.value = 1;
        
        masterVolumeNumberText.text = ((int)(masterVolumeSlider.value * 100)).ToString();
        musicVolumeNumberText.text = ((int)(musicVolumeSlider.value * 100)).ToString();
        sfxVolumeNumberText.text = ((int)(sfxVolumeSlider.value * 100)).ToString();
        
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
        
        DontDestroyOnLoad(gameObject);
    }

    private void GameOverAndPauseUIController_OnMainMenuOpened(object sender, EventArgs e)
    {
        var savedVolume = PlayerPrefs.GetFloat("MasterVolume");
        masterVolumeSlider.value = savedVolume;
        mainMenuMusic.SetVolume(savedVolume);
        SetMasterVolume(savedVolume);
        
        var savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        musicVolumeSlider.value = savedMusicVolume;
        SetMusicVolume(savedMusicVolume);
        mainMenuMusic.SetVolume(savedMusicVolume);
        
        sfxVolumeSlider.value = 1;
    }


    public void OpenSettingsMenu()
    {
        mainMenuCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume; 
        PlayerPrefs.SetFloat("MasterVolume", volume);
        masterVolumeNumberText.text = ((int)(masterVolumeSlider.value * 100)).ToString();
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolumeNumberText.text = ((int)(musicVolumeSlider.value * 100)).ToString();
        mainMenuMusic.SetVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume); 
    }
    
    public void SetSfxVolume(float arg0)
    {
        sfxVolumeNumberText.text = ((int)(sfxVolumeSlider.value * 100)).ToString();
    }

    public void MuteButtonClicked()
    {
        AudioListener.volume = 0;
        PlayerPrefs.SetFloat("MasterVolume", 0);
        masterVolumeNumberText.text = 0.ToString();
        musicVolumeNumberText.text = 0.ToString();
        sfxVolumeNumberText.text = 0.ToString();

        masterVolumeSlider.value = 0;
        musicVolumeSlider.value = 0;
        sfxVolumeSlider.value = 0;
    }
}
