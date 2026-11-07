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

    private int currentWaveIndex = 0;
    private bool isWaveActive = false;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        if (isWaveActive)
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
