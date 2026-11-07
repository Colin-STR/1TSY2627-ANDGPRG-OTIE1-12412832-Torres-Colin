using UnityEngine;

[System.Serializable]
public struct EnemyGroup
{
    public GameObject enemyPrefab;
    public int count;
    public float spawnInterval;
}

[CreateAssetMenu(fileName = "WaveData", menuName = "TowerDefense/Wave Data")]
public class WaveData : ScriptableObject
{
    public EnemyGroup[] enemyGroups;
}
