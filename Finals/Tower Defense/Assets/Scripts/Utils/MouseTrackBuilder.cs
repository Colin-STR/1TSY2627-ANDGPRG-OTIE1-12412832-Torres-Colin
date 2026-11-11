using UnityEngine;
using UnityEngine.EventSystems;

public class MouseTrackBuilder : MonoBehaviour
{
    public static MouseTrackBuilder Instance;

    [Header("Height Constraints")]
    public float minElevatedHeight = 2.0f;

    [Header("Preview Setup")]
    public GameObject previewTowerPrefab;
    private GameObject currentPreview;
    private TowerData currentData;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (currentPreview != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                currentPreview.transform.position = hit.point;

                bool isElevated = hit.point.y >= minElevatedHeight;

                Renderer r = currentPreview.GetComponentInChildren<Renderer>();
                if (r != null)
                {
                    r.material.color = isElevated ? Color.green : Color.red;
                }

                if (Input.GetMouseButtonDown(0) && isElevated)
                {
                    if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                        return;

                    BuildTowerAt(hit.point);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
        }
    }

    public void SelectTowerToBuild(TowerData data)
    {
        currentData = data;
        if (currentPreview != null) Destroy(currentPreview);

        currentPreview = Instantiate(data.towerPrefab);
        Collider col = currentPreview.GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    void BuildTowerAt(Vector3 position)
    {
        if (GameManager.Instance.playerGold >= currentData.buildCost)
        {
            GameManager.Instance.AddGold(-currentData.buildCost);

            GameObject newTower = Instantiate(currentData.towerPrefab, position, Quaternion.identity);
            TowerController controller = newTower.GetComponent<TowerController>();
            if (controller != null)
            {
                controller.Initialize(currentData);
            }

            CancelPlacement();
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    void CancelPlacement()
    {
        if (currentPreview != null) Destroy(currentPreview);
        currentData = null;
    }
}
