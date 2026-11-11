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

    private TowerController controller;
    private Renderer towerRenderer;
    private Color originalColor;

    void Start()
    {
        controller = GetComponent<TowerController>();
        towerRenderer = GetComponentInChildren<Renderer>();
        if (towerRenderer != null)
        {
            originalColor = towerRenderer.material.color;
        }
    }

    void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        TryUpgradeTower();
    }

    public void TryUpgradeTower()
    {
        if (level >= maxLevel)
        {
            Debug.Log($"{gameObject.name} is already at Max Level ({maxLevel}!)");
            return;
        }

        int upgradeCost = Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(1.5f, level - 1));

        if (GameManager.Instance.playerGold >= upgradeCost)
        {
            GameManager.Instance.AddGold(-upgradeCost);
            level++;

            if (controller != null)
            {
                controller.UpgradeStats(damageMultiplier, rangeMultiplier, fireRateMultiplier);
            }

            Debug.Log($"{gameObject.name} upgraded to Level {level}! Cost was {upgradeCost} gold.");
            StartCoroutine(FlashUpgradeEffect());
        }
        else
        {
            Debug.Log($"Not enough gold to upgrade! Need {upgradeCost} gold.");
        }
    }

    System.Collections.IEnumerator FlashUpgradeEffect()
    {
        if (towerRenderer != null)
        {
            towerRenderer.material.color = Color.cyan;
            transform.localScale *= 1.1f;
            yield return new WaitForSeconds(0.2f);
            transform.localScale /= 1.1f;
            towerRenderer.material.color = originalColor;
        }
    }
}