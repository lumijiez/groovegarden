using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerPusher : MonoBehaviour
{
    [SerializeField] private float maxRadius = 5f;
    [SerializeField] private float radiusChangeDuration = 0.5f;

    private bool isPushing = false;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private PlayerControls playerControls;
    private Light2D light2D;

    private void Awake()
    {
        //playerControls = new PlayerControls();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        light2D = GetComponent<Light2D>();
    }

    private void Start()
    {
        spriteRenderer.transform.localScale = Vector3.zero;
        spriteRenderer.transform.rotation = Quaternion.identity;
    }

    public void PushAll()
    {
        if (circleCollider == null)
        {
            circleCollider = gameObject.AddComponent<CircleCollider2D>();
        }

        if (isPushing || Stamina.Instance.CurrentStamina < 1) return;
        SoundEngine.Instance.Play("magic");
        Stamina.Instance.UseStamina();
        circleCollider.enabled = true;
        StartCoroutine(ChangeRadiusScaleAndRotationOverTime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        enemy?.TakeDamage(10, PlayerController.Instance.transform);
        if (projectile)
        {
            Destroy(projectile.gameObject);
        }
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
            light2D.pointLightOuterRadius = newRadius * 5;
            light2D.pointLightInnerRadius = newRadius / 5;

            Vector3 newScale = Vector3.one * t;
            spriteRenderer.transform.localScale = newScale;

            float newRotation = 360f * t;
            spriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, newRotation);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        circleCollider.radius = 0;
        light2D.pointLightOuterRadius = 0;
        light2D.pointLightInnerRadius = 0;
        spriteRenderer.transform.localScale = Vector3.zero;
        spriteRenderer.transform.rotation = Quaternion.identity;
        circleCollider.enabled = false;
        isPushing = false;
    }
}
