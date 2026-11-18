using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    [Header("Upgrade Configuration")]
    public int level = 1;
    public int maxLevel = 3;
    public int baseUpgradeCost = 50;

    [Header("Scaling Multipliers per Level")]
    public float damageMultiplier = 1.3f;
    public float rangeMultiplier = 1.1f;
    public float fireRateMultiplier = 1.15f;

    [Header("Visual Prefabs per Level")]
    [Tooltip("Element 0 = Level 1, Element 1 = Level 2, Element 2 = Level 3")]
    public GameObject[] levelModels;

    private TowerController controller;
    private GameObject currentVisualModel;

    void Start()
    {
        controller = GetComponent<TowerController>();
        UpdateVisualModel();
    }

    public int GetUpgradeCost()
    {
        if (level >= maxLevel) return 0;
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(1.5f, level - 1));
    }

    public bool CanUpgrade()
    {
        return level < maxLevel;
    }

    public void UpgradeTower()
    {
        if (!CanUpgrade()) return;

        int cost = GetUpgradeCost();
        if (GameManager.Instance.playerGold >= cost)
        {
            GameManager.Instance.AddGold(-cost);
            level++;

            if (controller != null)
            {
                controller.UpgradeStats(damageMultiplier, rangeMultiplier, fireRateMultiplier);
            }

            UpdateVisualModel();

            Debug.Log($"{gameObject.name} successfully upgraded to Level {level}!");
        }
    }

    private void UpdateVisualModel()
    {
        if (currentVisualModel != null)
        {
            Destroy(currentVisualModel);
        }

        int index = level - 1;
        if (levelModels != null && index >= 0 && index < levelModels.Length && levelModels[index] != null)
        {
            currentVisualModel = Instantiate(levelModels[index], transform);
            currentVisualModel.transform.localPosition = Vector3.zero;
            currentVisualModel.transform.localRotation = Quaternion.identity;

            foreach (Collider col in currentVisualModel.GetComponentsInChildren<Collider>())
            {
                Destroy(col);
            }
        }

        if (controller != null && currentVisualModel != null)
        {
            Transform rotatingPart = currentVisualModel.transform.Find("Turret");
            if (rotatingPart == null) rotatingPart = currentVisualModel.transform;

            Transform foundFirePoint = currentVisualModel.transform.Find("FirePoint");
            if (foundFirePoint == null)
            {
                foundFirePoint = rotatingPart.Find("FirePoint");
                if (foundFirePoint == null) foundFirePoint = rotatingPart; 
            }

            controller.SetVisualModelReferences(rotatingPart, foundFirePoint);
        }
    }
}