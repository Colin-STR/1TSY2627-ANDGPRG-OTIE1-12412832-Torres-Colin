using System.Security.Cryptography;
using UnityEngine;

public class WorldPositionLogger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log($"[Click Coordinate] Object: {hit.collider.name} | Position: {hit.point} | Height (Y): {hit.point.y:F2}");
            }
        }
    }
}
