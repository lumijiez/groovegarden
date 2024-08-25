using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class MobPusher : MonoBehaviour
{
    [SerializeField] private float maxRadius = 10f;
    [SerializeField] private float radiusChangeDuration = 2f;

    private bool isPushing = false;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        circleCollider.radius = 0;
        spriteRenderer.transform.localScale = Vector3.zero;
        spriteRenderer.transform.rotation = Quaternion.identity;
        circleCollider.enabled = false;
        isPushing = false;
    }

    public void PushAll()
    {
        if (isPushing) return;
        circleCollider.enabled = true;
        StartCoroutine(ChangeRadiusScaleAndRotationOverTime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth enemy = collision.gameObject.GetComponent<PlayerHealth>();
        enemy?.TakeDamage(5, transform);
    }

    private IEnumerator ChangeRadiusScaleAndRotationOverTime()
    {
        isPushing = true;
        float initialRadius = 0;
        float timeElapsed = 0f;

        while (timeElapsed < radiusChangeDuration)
        {
            float t = timeElapsed / radiusChangeDuration;
            float newRadius = Mathf.Lerp(initialRadius, maxRadius, t);
            circleCollider.radius = newRadius;

            Vector3 newScale = Vector3.one * t;
            spriteRenderer.transform.localScale = newScale;

            float newRotation = 360f * t;
            spriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, newRotation);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        circleCollider.radius = 0;
        spriteRenderer.transform.localScale = Vector3.zero;
        spriteRenderer.transform.rotation = Quaternion.identity;
        circleCollider.enabled = false;
        isPushing = false;
    }
}
