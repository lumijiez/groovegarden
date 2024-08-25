using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrekBoss : MonoBehaviour, IEnemy
{

    [SerializeField] private MobPusher pusher;

    [SerializeField] private AudioClip shrekSound;

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private AudioSource shrekSoundSource;


    private float dashSpeedMultiplier = 3f;
    private TrailRenderer trailRenderer;
    private EnemyPathfinding enemyPathfinding;
    private bool isAttacking = false;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    readonly int FIST_ATTACK_HASH = Animator.StringToHash("FistAttack");

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        trailRenderer = GetComponent<TrailRenderer>();
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        shrekSoundSource = GetComponent<AudioSource>();

        shrekSoundSource.clip = shrekSound;
    }

    public void Push()
    {
        pusher.PushAll();
    }

    public void Attack()
    {
        shrekSoundSource.Play();
        if (Random.Range(0f, 1f) < 0.3f)
        { 
            myAnimator.SetTrigger(ATTACK_HASH);

            if (transform.position.x - PlayerController.Instance.transform.position.x < 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        } else
        {
            if (!isAttacking)
            {
                myAnimator.SetTrigger(FIST_ATTACK_HASH);
                isAttacking = true;
                enemyPathfinding.MoveTo(PlayerController.Instance.transform.position - transform.position);
                enemyPathfinding.SetMultiplier(dashSpeedMultiplier);
                trailRenderer.emitting = true;
                StartCoroutine(EndDash());
            }
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
