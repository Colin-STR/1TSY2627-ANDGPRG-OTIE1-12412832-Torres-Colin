using UnityEngine;
public enum TowerType { Arrow, Cannon, Ice, Fire }

[CreateAssetMenu(fileName = "TowerData", menuName = "TowerDefense/Tower Data")]
public class TowerData : ScriptableObject
{
    public TowerType towerType;
    public string towerName;
    public int buildCost = 100;
    public float range = 5f;
    public float fireRate = 1f; // Attacks per second
    public float damage = 20f;

    [Header("Special Effects")]
    public bool isSplash = false;
    public float splashRadius = 2f;
    public bool slowsTarget = false;
    public float slowMultiplier = 0.5f; // e.g., 50% speed
    public float slowDuration = 2f;
    public bool appliesBurning = false;
    public float burnDamage = 5f;
    public float burnDuration = 3f;
    public bool targetsFlying = true;

    public GameObject towerPrefab; // 3D model representation
}