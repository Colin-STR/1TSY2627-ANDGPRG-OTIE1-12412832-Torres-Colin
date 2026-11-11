using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum EnemyType { Ground, Flying, Boss }

public class Enemy : MonoBehaviour
{
    [Header("Classification")]
    public EnemyType enemyType = EnemyType.Ground;

    [Header("Stats")]
    public float baseHp = 100f;
    public float currentHp;
    public int baseGoldDrop = 10;
    public float moveSpeed = 3.5f;
    public float flyingAltitude = 5f;

    private NavMeshAgent agent;
    private Transform targetCore;
    private int waveNumber;

    private float originalSpeed;
    private bool isSlowed = false;

    void Awake()
    {
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        if (nav != null) originalSpeed = nav.speed;
    }

    public void Initialize(Transform core, float hpMultiplier, int wave)
    {
        targetCore = core;
        waveNumber = wave;

        currentHp = baseHp * hpMultiplier;
        if (enemyType == EnemyType.Boss) currentHp *= 5f;

        agent = GetComponent<NavMeshAgent>();

        if (enemyType == EnemyType.Flying)
        {
            if (agent != null) agent.enabled = false;
            transform.position += Vector3.up * flyingAltitude;
        }
        else if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.SetDestination(targetCore.position);
        }
    }

    void Update()
    {
        if (enemyType == EnemyType.Flying)
        {
            Vector3 targetPos = new Vector3(targetCore.position.x, transform.position.y, targetCore.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.5f)
            {
                ReachCore();
            }
        }
        else if (agent != null && agent.enabled)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    ReachCore();
                }
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        if (currentHp <= 0) Die();
    }

    void Die()
    {
        int reward = waveNumber * baseGoldDrop;
        GameManager.Instance.AddGold(reward);
        Destroy(gameObject);
    }

    void ReachCore()
    {
        int damage = (enemyType == EnemyType.Boss) ? 3 : 1;
        GameManager.Instance.DamageCore(damage);
        Destroy(gameObject);
    }

    public void ApplySlow(float slowMultiplier, float duration)
    {
        if (!isSlowed)
        {
            StartCoroutine(SlowRoutine(slowMultiplier, duration));
        }
    }

    IEnumerator SlowRoutine(float multiplier, float duration)
    {
        isSlowed = true;
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        if (nav != null) nav.speed = originalSpeed * multiplier;

        yield return new WaitForSeconds(duration);

        if (nav != null) nav.speed = originalSpeed;
        isSlowed = false;
    }

    public void ApplyBurn(float burnDamage, float duration)
    {
        StartCoroutine(BurnRoutine(burnDamage, duration));
    }

    IEnumerator BurnRoutine(float damagePerTick, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            TakeDamage(damagePerTick);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
    }
}
