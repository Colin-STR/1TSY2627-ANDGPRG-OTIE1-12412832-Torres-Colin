using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSelectionManager : MonoBehaviour
{
    public static TowerSelectionManager Instance;
    private TowerUpgrade selectedTower;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                TowerUpgrade upgradeComp = hit.collider.GetComponent<TowerUpgrade>();
                if (upgradeComp != null)
                {
                    SelectTower(upgradeComp);
                }
                else
                {
                    DeselectTower();
                }
            }
        }
    }

    void SelectTower(TowerUpgrade tower)
    {
        selectedTower = tower;
        Debug.Log($"Selected tower: {tower.gameObject.name}");
        TowerUpgradeUI.Instance.ShowPanel(selectedTower);
    }

    void DeselectTower()
    {
        selectedTower = null;
        TowerUpgradeUI.Instance.HidePanel();
    }
}
