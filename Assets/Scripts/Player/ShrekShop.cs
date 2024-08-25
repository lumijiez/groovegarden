using System.Collections;
using UnityEngine;

public class ShrekShop : Singleton<ShrekShop>
{
    [SerializeField] private GameObject shrekShopUI;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject notKilledUI;
    [SerializeField] private AnimationCurve openingCurve;

    public bool isOpen { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public void EnableUI()
    {
        isOpen = true;
        if (VariableManager.Instance.wasShrekKilled)
        {
            notKilledUI.SetActive(false);
            mainUI.SetActive(true);
        } else
        {
            notKilledUI.SetActive(true);
            mainUI.SetActive(false);
        }
        InputManager.Instance.playerControls.Combat.Disable();
        StartCoroutine(OpenUICoroutine());
    }

    public void DisableUI()
    {
        isOpen = false;
        InputManager.Instance.playerControls.Combat.Enable();
        StartCoroutine(CloseUICoroutine());
    }

    private IEnumerator OpenUICoroutine()
    {
        float duration = 0.25f;
        float elapsed = 0f;
        RectTransform rectTransform = shrekShopUI.GetComponent<RectTransform>();

        shrekShopUI.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = openingCurve.Evaluate(elapsed / duration);
            rectTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        rectTransform.localScale = Vector3.one;
    }

    private IEnumerator CloseUICoroutine()
    {
        float duration = 0.1f;
        float elapsed = 0f;
        RectTransform rectTransform = shrekShopUI.GetComponent<RectTransform>();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = 1 - (elapsed / duration);
            rectTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        rectTransform.localScale = Vector3.zero;
        shrekShopUI.SetActive(false);
    }
}
