using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Terrain targetTerrain;
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public float moveSpeed = 3f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    [Header("Wander Settings")]
    public float wanderRadius = 8f;
    public float wanderSpeed = 1.5f;
    public float wanderPauseTime = 2f;
    public float wanderPointTolerance = 0.5f;

    public AudioSource audioSource;
    public AudioClip spotPlayerSound;

    private Animator animator;
    private float attackTimer;
    private PlayerHealth playerHealth;
    private bool hasSpottedPlayer;

    private Vector3 spawnPosition;
    private Vector3 wanderTarget;
    private float wanderPauseTimer;
    private bool hasWanderTarget;

    void Start()
    {
        animator = GetComponent<Animator>();
        spawnPosition = transform.position;

        if (targetTerrain == null)
        {
            targetTerrain = Terrain.activeTerrain;
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = distance <= detectionRange;

        if (canSeePlayer && !hasSpottedPlayer)
        {
            hasSpottedPlayer = true;

            if (audioSource != null && spotPlayerSound != null)
            {
                audioSource.PlayOneShot(spotPlayerSound);
            }
        }
        else if (!canSeePlayer && hasSpottedPlayer)
        {
            hasSpottedPlayer = false;
        }

        bool isMoving = false;

        if (canSeePlayer)
        {
            if (distance > attackRange)
            {
                ChasePlayer();
                isMoving = true;
            }
            else
            {
                AttackPlayer();
            }
        }
        else
        {
            isMoving = Wander();
        }

        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
        }

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private float SampleGroundHeight(Vector3 worldPos)
    {
        if (targetTerrain == null)
        {
            return worldPos.y;
        }

        return targetTerrain.SampleHeight(worldPos) + targetTerrain.transform.position.y;
    }

    private bool Wander()
    {
        if (!hasWanderTarget)
        {
            if (wanderPauseTimer > 0f)
            {
                wanderPauseTimer -= Time.deltaTime;
                return false;
            }

            PickNewWanderTarget();
        }

        float distanceToTarget = Vector3.Distance(transform.position, wanderTarget);

        if (distanceToTarget <= wanderPointTolerance)
        {
            hasWanderTarget = false;
            wanderPauseTimer = wanderPauseTime;
            return false;
        }

        Vector3 direction = wanderTarget - transform.position;
        direction.y = 0f;
        direction.Normalize();

        Vector3 newPos = transform.position + direction * wanderSpeed * Time.deltaTime;
        newPos.y = SampleGroundHeight(newPos);

        transform.position = newPos;
        transform.rotation = Quaternion.LookRotation(direction);

        return true;
    }

    private void PickNewWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = spawnPosition + new Vector3(randomCircle.x, 0f, randomCircle.y);
        hasWanderTarget = true;
    }

    private void ChasePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        Vector3 newPos = transform.position + direction * moveSpeed * Time.deltaTime;
        newPos.y = SampleGroundHeight(newPos);

        transform.position = newPos;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void AttackPlayer()
    {
        Debug.Log($"AttackPlayer called, playerHealth is null: {playerHealth == null}");
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (attackTimer <= 0f && playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            attackTimer = attackCooldown;

            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }
    }
}