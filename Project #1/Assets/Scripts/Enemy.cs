using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public int health = 2;
    private Transform player;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        direction.Normalize();
        rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);

        if (direction != Vector3.zero)
            rb.MoveRotation(Quaternion.LookRotation(direction));
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyDeathSound);
            FindObjectOfType<WaveManager>().RegisterKill();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Hit!");
        }
    }
}
