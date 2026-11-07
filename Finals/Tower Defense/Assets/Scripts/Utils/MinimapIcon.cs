using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    public SpriteRenderer iconRenderer;

    public void SetupIcon(EnemyType type)
    {
        if (iconRenderer == null) return;

        switch (type)
        {
            case EnemyType.Ground:
                iconRenderer.color = Color.red;
                break;
            case EnemyType.Flying:
                iconRenderer.color = Color.cyan;
                break;
            case EnemyType.Boss:
                iconRenderer.color = Color.yellow;
                transform.localScale *= 1.5f;
                break;
        }
    }
}
