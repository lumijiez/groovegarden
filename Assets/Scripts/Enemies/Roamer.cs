using System.Collections;
using UnityEngine;

public class Roamer : MonoBehaviour, IEnemy
{
    private float dashSpeedMultiplier = 2f;
    private TrailRenderer trailRenderer;
    private EnemyPathfinding enemyPathfinding;
    private bool isAttacking = false;

    [SerializeField] private AudioClip attackSound;
    private AudioSource myAudioSource;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        trailRenderer = GetComponent<TrailRenderer>();
        myAudioSource = GetComponent<AudioSource>();
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            myAudioSource.clip = attackSound;
            myAudioSource.Play();
            isAttacking = true;
            enemyPathfinding.MoveTo(PlayerController.Instance.transform.position - transform.position);
            enemyPathfinding.SetMultiplier(dashSpeedMultiplier);
            trailRenderer.emitting = true;
            StartCoroutine(EndDash());
        }
    }

    private IEnumerator EndDash()
    {      
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
        enemyPathfinding.SetMultiplier(1f);
        trailRenderer.emitting = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(2, transform);
            }
            enemyPathfinding.StopMoving();
            enemyPathfinding.SetMultiplier(1f);
            trailRenderer.emitting = false;
        }
    }
}
