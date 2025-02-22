using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    enum State
    {
        Idle,
        Attack
    }

    private State state;

    [Header("Elements")]
    [SerializeField] private Transform hitDetetionTranform;
    [SerializeField] private float hitDetectionRadius;


    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private Animator animator;
    [SerializeField] private float AttackDelay;
    private List<Enemy> damagedEnemies = new List<Enemy>();
    private float attackTimer;

    [Header("Settings")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float aimLerp;
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                AutoAim();
                break;
            case State.Attack:
                Attacking();
                break;
        }
       
    }

    private void AutoAim()
    {
        
        Enemy closestEnemy = GetClosestEnemy();
        Vector3 targetUpVector = Vector3.up;


        if (closestEnemy != null)
        {
            Debug.Log("danh danh");
            ManageAttack();
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);

        IncrementAttackTimer();
    }

    private void ManageAttack()
    {

        if (attackTimer >= AttackDelay)
        {
            attackTimer = 0;
            StartAttack();
        }
    }

    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }

    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        animator.speed = 1f / AttackDelay;
        animator.Play("Attack");
        state = State.Attack;
        damagedEnemies.Clear();
    }

    private void Attacking()
    {
        Attack();
    }

    private void StopAttack()
    {
        state = State.Idle;
        damagedEnemies.Clear();
    }

    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(hitDetetionTranform.position, hitDetectionRadius, enemyMask);
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();
            if (!damagedEnemies.Contains(enemy))
            {
                enemy.TakeDamage(damage);
                damagedEnemies.Add(enemy);
            }
            
        }
    }

    private Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);

        int enemyCount = enemies.Length;
        float minDistance = range;

        if (enemyCount <= 0) return null;

        for (int i = 0; i < enemyCount; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>();
            float distanceToEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);

            if (distanceToEnemy < minDistance)
            {
                closestEnemy = enemyChecked;
                minDistance = distanceToEnemy;
            }

        }
        return closestEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitDetetionTranform.position, hitDetectionRadius);
    }
}
