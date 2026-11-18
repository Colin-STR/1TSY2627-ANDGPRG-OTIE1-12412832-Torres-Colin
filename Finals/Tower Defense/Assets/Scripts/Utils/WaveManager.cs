using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Setup")]
    public List<WaveData> waves;
    public Transform spawnPoint;
    public Transform coreTransform;

    [Header("UI Display")]
    public TMP_Text waveText;
    public GameObject startFirstWaveButton;

    private int currentWaveIndex = 0;
    private bool isWaveActive = false;
    private bool gameStarted = false;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        if (startFirstWaveButton != null)
        {
            startFirstWaveButton.SetActive(true);
        }

        if (waveText != null)
        {
            waveText.text = "Press Start to Begin";
        }
    }
    public void StartFirstWaveManually()
    {
        if (!gameStarted)
        {
            gameStarted = true;

            if (startFirstWaveButton != null)
            {
                startFirstWaveButton.SetActive(false);
            }

            TriggerWave();
        }
    }

    void Update()
    {
        if (gameStarted && isWaveActive)
        {
            spawnedEnemies.RemoveAll(e => e == null);

            if (spawnedEnemies.Count == 0)
            {
                isWaveActive = false;
                if (currentWaveIndex < waves.Count)
                {
                    Invoke(nameof(TriggerWave), 3f);
                }
                else
                {
                    Debug.Log("Final Wave Cleared! Victory!");
                    if (waveText != null) waveText.text = "Victory!";

                    if (AudioManager.Instance != null && AudioManager.Instance.victoryMusic != null)
                    {
                        AudioManager.Instance.PlayBGM(AudioManager.Instance.victoryMusic, false);
                    }

                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.TriggerVictory();
                    }
                }
            }
        }
    }

    void TriggerWave()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        isWaveActive = true;
        currentWaveIndex++;

        if (waveText != null)
            waveText.text = $"Wave {currentWaveIndex} / {waves.Count}";

        float hpScaleMultiplier = 1f + (currentWaveIndex - 1) * 0.15f;
        WaveData currentWave = waves[currentWaveIndex - 1];

        foreach (EnemyGroup group in currentWave.enemyGroups)
        {
            for (int i = 0; i < group.count; i++)
            {
                GameObject obj = Instantiate(group.enemyPrefab, spawnPoint.position, Quaternion.identity);
                Enemy enemyScript = obj.GetComponent<Enemy>();

                if (enemyScript != null)
                {
                    enemyScript.Initialize(coreTransform, hpScaleMultiplier, currentWaveIndex);
                }

                spawnedEnemies.Add(obj);
                yield return new WaitForSeconds(group.spawnInterval);
            }
        }
    }
}