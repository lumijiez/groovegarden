using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15f;

    [SerializeField] private AudioClip deathSound;

    private int currentHealth;
    private Knockback knockback;
    private Flash flash;
    private AudioSource myAudioSource;

    private void Awake() {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
        myAudioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    public void TakeDamage(int damage, Transform source)
    {
        currentHealth -= damage;
        knockback.GetKnockedBack(source, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine() {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    public void DetectDeath() {
        if (currentHealth <= 0) {
            if (myAudioSource != null && deathSound != null)
            {
                myAudioSource.clip = deathSound;
                myAudioSource.Play();
            }
            if (GetComponent<ShrekBoss>() != null)
            {
                VariableManager.Instance.wasShrekKilled = true;
            }
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            GetComponent<PickUpSpawner>().DropItems();
            Destroy(gameObject);
        }
    }
}
