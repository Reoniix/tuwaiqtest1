using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ZombieAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackRate = 1f; // time between attacks

    [Header("Audio")]
    public AudioSource audioSource;
    [Range(0f, 1f)]
    public float attackVolume = 0.5f;  // slider in inspector for punch AND oof audio
    
    public AudioClip attackSFX;

    NavMeshAgent agent;
    float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time < nextAttackTime) return;

        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph == null)
        {
            Debug.Log("No PlayerHealth on Player!");
            return;
        }

        // STOP movement so zombie doesn't overlap/push inside player
        agent.isStopped = true;

        // Deal damage + knockback
        ph.TakeDamage(damage, transform.position);

        // Play attack sound at adjustable volume
        if (audioSource != null && attackSFX != null)
            audioSource.PlayOneShot(attackSFX, attackVolume);

        // Resume movement after half a second
        StartCoroutine(ResumeMovement());

        nextAttackTime = Time.time + attackRate;
    }

    IEnumerator ResumeMovement()
    {
        yield return new WaitForSeconds(0.5f);
        agent.isStopped = false;
    }
}
