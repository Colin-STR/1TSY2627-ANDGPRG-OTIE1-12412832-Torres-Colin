using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 25f;
    public float edgeBoundary = 15f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 10f;
    public float minOrthographicSize = 5f;
    public float maxOrthographicSize = 25f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 moveDir = Vector3.zero;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        moveDir += new Vector3(x, 0f, z);

        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x >= Screen.width - edgeBoundary) moveDir.x += 1f;
        if (mousePos.x <= edgeBoundary) moveDir.x -= 1f;
        if (mousePos.y >= Screen.height - edgeBoundary) moveDir.z += 1f;
        if (mousePos.y <= edgeBoundary) moveDir.z -= 1f;

        transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.World);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f && cam != null && cam.orthographic)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minOrthographicSize, maxOrthographicSize);
        }
    }
}
