using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Public Static Objects
    public static event EventHandler OnScoreChanged;
    public static event EventHandler OnLivesChanged;
    public static GameManager Instance { get; private set; }
    

    // External Properties
    public int Score { get; private set; }
    public int Lives { get; private set; }

    
    // Editor Fields

    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private int numberOfLives = 3;
    [SerializeField] private int respawnDelay = 3;
    [SerializeField] private float gameResetDelay = 5.0f;
    
    // Internal variables 
    private bool _gameIsActive = true;

    // Unity Events
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }    
    private void Start()
    {
        // Set the number of lives to match what was chosen at design time
        Lives = numberOfLives;
        
        player = Instantiate(player, Vector3.zero, Quaternion.identity);
        
        // It seems that one prefab instance is enough for this simple game
        explosion = Instantiate(explosion, Vector3.zero, Quaternion.identity);
        
        // Start the asteroid spawner
        AsteroidSpawner.Instance.StartSpawning();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            UIManager.Instance.UpdateHelpInfoPanel();
        
        if (Input.GetKeyDown(KeyCode.Escape) && _gameIsActive && !UIManager.Instance.SettingsPanelVisible)
            UIManager.Instance.ShowGameOverPanel(!UIManager.Instance.GameOverPanelVisible);
        
        if (Input.GetKeyDown(KeyCode.Escape) && _gameIsActive && UIManager.Instance.SettingsPanelVisible)
            UIManager.Instance.ShowSettingsPanel(false);
        
        if (Input.GetKeyDown(KeyCode.Tab) && !UIManager.Instance.GameOverPanelVisible)
            UIManager.Instance.ShowSettingsPanel(!UIManager.Instance.SettingsPanelVisible);
    }

    // Internal Functions
    private void GameOver()
    {
        _gameIsActive = false;
        
        AsteroidSpawner.Instance.StopSpawning();
        UIManager.Instance.ShowGameOverDisplay();
    }

    private void ResetGame()
    {
        var currentAsteroidArray = FindObjectsByType<Asteroid>(FindObjectsSortMode.None);
        foreach (var asteroid in currentAsteroidArray)
        {
            Destroy(asteroid.gameObject);
        }
        
        Score = 0;
        Lives = numberOfLives;
        OnScoreChanged?.Invoke(this, EventArgs.Empty);
        OnLivesChanged?.Invoke(this, EventArgs.Empty);
        
        player.gameObject.SetActive(true);
        
        // Start the asteroid spawner again
        AsteroidSpawner.Instance.StartSpawning();
        _gameIsActive = true;
    }
    private void RespawnPlayer()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
    }

    private void PlayExplosion(Vector3 position)
    {
        explosion.transform.position = position;
        explosion.Play();
    }
    
    // External Functions
    public void AsteroidHasBeenDestroyed(int pointsPerKill, Vector3 position)
    {
        Score += pointsPerKill;
        OnScoreChanged?.Invoke(this, EventArgs.Empty);
        PlayExplosion(position);
    }

    public void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }
    public void PlayerHasDied()
    {
        Lives--;
        
        PlayExplosion(player.transform.position);
        OnLivesChanged?.Invoke(this, EventArgs.Empty);
        
        if (Lives > 0)
        {
            Invoke(nameof(RespawnPlayer), respawnDelay);
            
            return; // Make sure we don't call GameOver()
        }
        
        // If we get here the game is over
        GameOver();
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif        
    }

    public void PlayAgain()
    {
        UIManager.Instance.ShowGameOverPanel(false);
        Invoke(nameof(ResetGame), gameResetDelay);
    }

}
