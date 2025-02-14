using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player player;

    [Header("Spawn Squence Related")]
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer spawnIndicator;

    private bool hasSpawned;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float playerDetectionRadius;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private float attackDelay;
    private float attackTimer;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticles;

    [Header("Debug")]
    [SerializeField] private bool gizmos;


    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.Log("No found player, auto destroying...");
            Destroy(gameObject);
        }

        renderer.enabled = false;
        spawnIndicator.enabled = true;

        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f).
            setLoopPingPong(4).
            setOnComplete(SpawnSequenceCompleted);

        attackDelay = 1f / attackFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasSpawned) return;

        FollowPlayer();

        if (attackTimer >= attackDelay)
        {
            TryAttack();
        }
        else
        {
            Wait();
        }
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = targetPosition;
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
        attackTimer = 0;
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

    private void SpawnSequenceCompleted()
    {
        renderer.enabled = true;
        spawnIndicator.enabled = false;
        hasSpawned = true;
    }

    private void OnDrawGizmos()
    {
        if (!gizmos) return; 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
