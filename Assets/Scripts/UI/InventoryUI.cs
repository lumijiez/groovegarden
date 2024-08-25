using System.Collections;
using UnityEngine;

public class InventoryUI : Singleton<InventoryUI>
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private AnimationCurve openingCurve;
    private bool isUIOpen = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InputManager.Instance.playerControls.Inventory.OpenUI.performed += _ => ToggleUI();
    }

    private void ToggleUI()
    {
        if (ShrekShop.Instance.isOpen) return;

        if (isUIOpen)
        {
            StartCoroutine(CloseUICoroutine());
        }
        else
        {
            StartCoroutine(OpenUICoroutine());
        }
    }

    public void EnableUI()
    {
        StartCoroutine(OpenUICoroutine());
    }

    public void DisableUI()
    {
        StartCoroutine(CloseUICoroutine());
    }

    private IEnumerator OpenUICoroutine()
    {
        float duration = 0.25f;
        float elapsed = 0f;
        RectTransform rectTransform = inventoryUI.GetComponent<RectTransform>();

        inventoryUI.SetActive(true);
        InputManager.Instance.playerControls.Combat.Disable();
        InputManager.Instance.playerControls.Movement.Disable();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = openingCurve.Evaluate(elapsed / duration);
            rectTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        rectTransform.localScale = Vector3.one; 
        isUIOpen = true;
    }

    private IEnumerator CloseUICoroutine()
    {
        float duration = 0.1f;
        float elapsed = 0f;
        RectTransform rectTransform = inventoryUI.GetComponent<RectTransform>();

        InputManager.Instance.playerControls.Combat.Enable();
        InputManager.Instance.playerControls.Movement.Enable();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = 1 - (elapsed / duration);
            rectTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        rectTransform.localScale = Vector3.zero;
        inventoryUI.SetActive(false);
        isUIOpen = false;
    }
}
