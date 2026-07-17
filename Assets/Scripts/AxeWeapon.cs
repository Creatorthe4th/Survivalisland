using UnityEngine;
using System.Collections;

public class AxeWeapon : MonoBehaviour
{
    public Transform hitOrigin;
    public float hitRadius = 1.2f;
    public int attackDamage = 15;
    public float swingToImpactDelay = 0.3f;
    public float attackCooldown = 0.8f;
    public string attackAnimationTrigger = "Attack";

    public AudioSource audioSource;
    public AudioClip playerAttackSound;

    private Animator animator;
    private bool isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (hitOrigin == null)
        {
            hitOrigin = transform;
        }
    }

    public void TryAttack()
    {
        if (isAttacking)
        {
            return;
        }

        StartCoroutine(SwingAxe());
    }

    private IEnumerator SwingAxe()
    {
        isAttacking = true;

        if (animator != null)
        {
            animator.SetTrigger(attackAnimationTrigger);
        }

        yield return new WaitForSeconds(swingToImpactDelay);

        CheckForHit();

        yield return new WaitForSeconds(attackCooldown - swingToImpactDelay);

        isAttacking = false;
    }

    private void CheckForHit()
{
    Collider[] hits = Physics.OverlapSphere(hitOrigin.position, hitRadius);
    Debug.Log($"Axe hit check found {hits.Length} colliders");
    bool hitEnemy = false;

    foreach (var hit in hits)
    {
        EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(attackDamage);
            hitEnemy = true;
        }
    }

    if (hitEnemy && audioSource != null && playerAttackSound != null)
    {
        audioSource.PlayOneShot(playerAttackSound);
    }
}

void OnDrawGizmosSelected()
{
    if (hitOrigin != null)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitOrigin.position, hitRadius);
    }
}
}