using UnityEngine;

public class TowerController : MonoBehaviour
{
    private TowerData data;
    private float fireCooldown = 0f;
    private float currentDamage;
    private float currentRange;
    private float currentFireRate;
    private float currentSplashRadius;

    public void Initialize(TowerData towerData)
    {
        data = towerData;
        currentDamage = data.damage;
        currentRange = data.range;
        currentFireRate = data.fireRate;
        currentSplashRadius = data.splashRadius;
    }

    public void UpgradeStats(float dmgMult, float rangeMult, float speedMult)
    {
        currentDamage *= dmgMult;
        currentRange *= rangeMult;
        currentFireRate *= speedMult;
        Debug.Log($"Stats Upgraded! New Damage: {currentDamage}, New Range: {currentRange}");
    }

    void Update()
    {
        if (data == null) return;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            FindAndAttackTarget();
            fireCooldown = 1f / currentFireRate;
        }
    }

    void FindAndAttackTarget()
    {
        if (data == null) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, currentRange);
        Transform bestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (!data.targetsFlying && enemy.enemyType == EnemyType.Flying) continue;

                    float distanceToEnemy = Vector3.Distance(transform.position, hit.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        bestTarget = hit.transform;
                    }
                }
            }
        }

        if (bestTarget != null)
        {
            ExecuteAttack(bestTarget);
        }
    }

    void ExecuteAttack(Transform target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy == null) return;

        if (data.isSplash)
        {
            Collider[] splashHits = Physics.OverlapSphere(target.position, currentSplashRadius);
            foreach (Collider hit in splashHits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Enemy e = hit.GetComponent<Enemy>();
                    if (e != null) ApplyDamageAndEffects(e);
                }
            }
        }
        else
        {
            ApplyDamageAndEffects(enemy);
        }
    }

    void ApplyDamageAndEffects(Enemy enemy)
    {
        enemy.TakeDamage(currentDamage);

        if (data.slowsTarget)
        {
            enemy.ApplySlow(data.slowMultiplier, data.slowDuration);
        }

        if (data.appliesBurning)
        {
            enemy.ApplyBurn(data.burnDamage, data.burnDuration);
        }
    }

    void OnDrawGizmosSelected()
    {
        float rangeToDraw = (data != null) ? currentRange : 5f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeToDraw);
    }
}