using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    // Static Fields
    public static UIManager Instance { get; private set; }
    
    // Public Fields
    public bool GameOverPanelVisible => gameOverPanel.activeSelf;
    public bool SettingsPanelVisible => settingsPanel.activeSelf;
    
    // Editor Fields
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    
    [Header("Panels")]
    [SerializeField] private GameObject helpInfoPanel;
    
    [Header("Game Over Panel")]
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private GameObject gameOverPanel;
    
    [Header("Settings Panel")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundEffectsVolumeSlider;
    [SerializeField] private GameObject settingsPanel;
    
    // Unity Events
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DisplayScore();
        gameOverText.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
        
        ShowGameOverPanel(false);
        ShowSettingsPanel(false);
        musicVolumeSlider.value = AudioManager.Instance.MusicVolume;
        soundEffectsVolumeSlider.value = AudioManager.Instance.EffectsVolume;
    }


    private void OnEnable()
    {
        GameManager.OnScoreChanged += GameManager_OnScoreChanged;
        GameManager.OnLivesChanged += GameManager_OnLivesChanged;
    }
    
    private void OnDisable()
    {
        GameManager.OnScoreChanged -= GameManager_OnScoreChanged;
        GameManager.OnLivesChanged -= GameManager_OnLivesChanged;
    }
    
    // Internal Functions 
    private void DisplayScore()
    {
        scoreText.text = GameManager.Instance.Score.ToString();
    }

    private void DisplayLives()
    {
        var lives = GameManager.Instance.Lives;
        livesText.text = $"Lives: {lives}";
    }


    // Event Notifications
    private void GameManager_OnScoreChanged(object sender, EventArgs e)
    {
        DisplayScore();
    }

    private void GameManager_OnLivesChanged(object sender, EventArgs e)
    {
        DisplayLives();
    }
    
    // Public Functions

    public void OnMusicVolumeChanged()
    {
        AudioManager.Instance.MusicVolume = musicVolumeSlider.value;
    }

    public void ShowGameOverPanel(bool show)
    {
        gameOverPanel.SetActive(show);

        GameManager.Instance.PauseGame(show);
    }

    public void ShowSettingsPanel(bool show)
    {
        settingsPanel.SetActive(show);
        GameManager.Instance.PauseGame(show);
    }

    public void ShowGameOverDisplay()
    {
        gameOverText.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(true);
        ShowGameOverPanel(true);
    }

    public void ResumeGame()
    {
        GameManager.Instance.PauseGame(false);
        gameOverPanel.SetActive(false);
    }
    public void UpdateHelpInfoPanel()
    {
        // Toggle the visibility of the help info panel
        helpInfoPanel.SetActive(!helpInfoPanel.activeSelf);
    }


}
