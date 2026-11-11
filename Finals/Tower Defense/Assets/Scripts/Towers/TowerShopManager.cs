using UnityEngine;

public class TowerShopManager : MonoBehaviour
{
    public static TowerShopManager Instance;

    private BuildSlot selectedSlot;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenBuildMenu(BuildSlot slot)
    {
        selectedSlot = slot;
        Debug.Log("Build slot selected! Display Tower Selection UI.");      
    }

    public void TryPurchaseAndBuild(TowerData towerData)
    {
        if (selectedSlot == null) return;

        if (GameManager.Instance.playerGold >= towerData.buildCost)
        {
            GameManager.Instance.AddGold(-towerData.buildCost); 
            selectedSlot.BuildTower(towerData.towerPrefab, towerData);
            selectedSlot = null;
        }
        else
        {
            Debug.Log("Not enough gold to build this tower!");
        }
    }
}
