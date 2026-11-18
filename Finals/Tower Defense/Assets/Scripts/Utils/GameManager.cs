using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    public int playerGold = 150;
    public int coreHealth = 20;

    [Header("UI References")]
    public TMP_Text goldText;
    public TMP_Text coreHealthText;
    public GameObject defeatPanel;
    public GameObject victoryPanel;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (defeatPanel != null) defeatPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        playerGold += amount;
        UpdateUI();
    }

    public void DamageCore(int damage)
    {
        if (isGameOver) return;

        coreHealth -= damage;
        if (coreHealth < 0) coreHealth = 0;

        UpdateUI();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.playerHitSound);
        }

        if (coreHealth <= 0)
        {
            TriggerDefeat();
        }
    }

    void UpdateUI()
    {
        if (goldText != null) goldText.text = $"Gold: {playerGold}";
        if (coreHealthText != null) coreHealthText.text = $"Core HP: {coreHealth}";
    }

    public void TriggerDefeat()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;

        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBGM();
            if (AudioManager.Instance.gameOverMusic != null)
            {
                AudioManager.Instance.bgmSource.PlayOneShot(AudioManager.Instance.gameOverMusic);
            }
        }

        Debug.Log("Game Over! Core destroyed.");
    }

    public void TriggerVictory()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        Debug.Log("Victory! All waves cleared.");
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu(string menuSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}