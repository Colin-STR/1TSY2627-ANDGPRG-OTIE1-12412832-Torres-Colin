using UnityEngine;

public class BuildSlot : MonoBehaviour
{
    [HideInInspector]
    public GameObject builtTower = null;
    private Renderer slotRenderer;
    private Color originalColor;

    void Start()
    {
        slotRenderer = GetComponent<Renderer>();
        if (slotRenderer != null)
            originalColor = slotRenderer.material.color;
    }

    void OnMouseDown()
    {
        if (builtTower == null)
        {
            TowerShopManager.Instance.OpenBuildMenu(this);
        }
        else
        {
            Debug.Log("Slot occupied!");
        }
    }

    public void BuildTower(GameObject towerPrefab, TowerData data)
    {
        builtTower = Instantiate(towerPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        TowerController controller = builtTower.GetComponent<TowerController>();
        if (controller != null)
        {
            controller.Initialize(data);
        }
    }

    void OnMouseEnter()
    {
        if (slotRenderer != null && builtTower == null)
            slotRenderer.material.color = Color.green;
    }

    void OnMouseExit()
    {
        if (slotRenderer != null)
            slotRenderer.material.color = originalColor;
    }
}
