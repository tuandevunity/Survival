using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header("Components")]
    private EnemyMovement movement;


    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private TextMeshPro healthText;    
    private int health;

    [Header("Elements")]
    private Player player;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float playerDetectionRadius;

    [Header("Spawn Squence Related")]
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer spawnIndicator;


    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private float attackDelay;
    private float attackTimer;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticles;

    [Header("Debug")]
    [SerializeField] private bool gizmos;


    private bool hasSpawned;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        if (healthText == null) return;
        healthText.text = health.ToString();
        movement = GetComponent<EnemyMovement>();
        player = FindFirstObjectByType<Player>();
        
        if (player == null)
        {
            Debug.Log("No found player, auto destroying...");
            Destroy(gameObject);
        }

        StartSpawnSequence();

        attackDelay = 1f / attackFrequency;
    }

    private void Update()
    {
        if (!hasSpawned) return;



        if (attackTimer >= attackDelay)
        {
            TryAttack();
        }
        else
        {
            Wait();
        }
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= playerDetectionRadius)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log($"dealing {damage} to player");
        player.TakeDamage(damage);
        attackTimer = 0;
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;
        healthText.text = health.ToString();

        if (health <= 0) PassAway();
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    private void PassAway()
    {
        passAwayParticles.transform.SetParent(null);
        passAwayParticles.Play();
        Destroy(gameObject);
    }


    private void StartSpawnSequence()
    {
        SetEnemyVisibility(false);

        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f).
            setLoopPingPong(4).
            setOnComplete(SpawnSequenceCompleted);
    }

    private void SpawnSequenceCompleted()
    {
        SetEnemyVisibility(true);
        hasSpawned = true;

        movement.StorePlayer(player);
    }

    private void SetEnemyVisibility(bool visibility)
    {
        renderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }

    private void OnDrawGizmos()
    {
        if (!gizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
    // Update is called once per frame
}
