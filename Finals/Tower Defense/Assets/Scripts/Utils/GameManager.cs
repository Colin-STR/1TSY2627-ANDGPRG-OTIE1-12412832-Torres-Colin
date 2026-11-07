using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Base Core Stats")]
    public int coreHealth = 20;
    public int playerGold = 500;

    [Header("UI References")]
    public TMP_Text healthText;
    public TMP_Text goldText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        playerGold += amount;
        UpdateUI();
    }

    public void DamageCore(int amount)
    {
        coreHealth -= amount;
        UpdateUI();

        if (coreHealth <= 0)
        {
            coreHealth = 0;
            Debug.Log("Game Over! The Core was destroyed.");
        }
    }

    private void UpdateUI()
    {
        if (healthText != null) healthText.text = $"Core HP: {coreHealth}";
        if (goldText != null) goldText.text = $"Gold: {playerGold}";
    }
}
