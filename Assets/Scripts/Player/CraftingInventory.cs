using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingInventory : Singleton<CraftingInventory>
{
    private int healPotionAmount = 0;
    [SerializeField] private TextMeshProUGUI healPotionAmountText;

    private int largerHealPotionAmount = 0;
    [SerializeField] private TextMeshProUGUI largerHealPotionAmountText;

    private int superHealPotionAmount = 0;
    [SerializeField] private TextMeshProUGUI superHealPotionAmountText;

    private int strengthPotionAmount = 0;
    [SerializeField] private TextMeshProUGUI strengthPotionAmountText;

    private int leavesAmount = 0;
    [SerializeField] private TextMeshProUGUI leavesAmountText;

    private int gelAmount = 0;
    [SerializeField] private TextMeshProUGUI gelAmountText;

    private int bucketsAmount = 0;
    [SerializeField] private TextMeshProUGUI bucketsAmountText;

    [SerializeField] private TextMeshProUGUI strengthTimerText;

    // Resources in Shop
    [SerializeField] private TextMeshProUGUI leavesAmountTextShop;

    [SerializeField] private TextMeshProUGUI gelAmountTextShop;

    [SerializeField] private TextMeshProUGUI bucketsAmountTextShop;

    protected override void Awake()
    {
        base.Awake();

        //healPotionAmountText = GameObject.Find("HealPotionAmount").GetComponent<TextMeshProUGUI>();
        //largerHealPotionAmountText = GameObject.Find("LargerHealPotionAmount").GetComponent<TextMeshProUGUI>();
        //superHealPotionAmountText = GameObject.Find("SuperHealPotionAmount").GetComponent<TextMeshProUGUI>();
        //leavesAmountText = GameObject.Find("LeavesAmount").GetComponent<TextMeshProUGUI>();
        //gelAmountText = GameObject.Find("GelAmount").GetComponent<TextMeshProUGUI>();
        //bucketsAmountText = GameObject.Find("BucketsAmount").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        InputManager.Instance.playerControls.Inventory.Heal.performed += _ => UseHeal();
        InputManager.Instance.playerControls.Inventory.Strength.performed += _ => UseStrength();
    }

    public void UseHeal()
    {
        if (superHealPotionAmount > 0)
        {
            superHealPotionAmount--;
            superHealPotionAmountText.text = superHealPotionAmount.ToString();
            PlayerHealth.Instance.HealPlayer(100);
        }

        if (largerHealPotionAmount > 0)
        {
            largerHealPotionAmount--;
            largerHealPotionAmountText.text = largerHealPotionAmount.ToString();
            PlayerHealth.Instance.HealPlayer(50);
        }

        if (healPotionAmount > 0)
        {
            healPotionAmount--;
            healPotionAmountText.text = healPotionAmount.ToString();
            PlayerHealth.Instance.HealPlayer(10);
        }
    }

    public void UseStrength()
    {
        if (strengthPotionAmount > 0)
        {
            superHealPotionAmount--;
            superHealPotionAmountText.text = superHealPotionAmount.ToString();
            StartCoroutine(StrengthCoroutine());
        }
    }

    private IEnumerator StrengthCoroutine()
    {
        VariableManager.Instance.weaponBonus = 5;
        float totalTime = 5 * 60;
        while (totalTime > 0)
        {
            totalTime -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(totalTime / 60);
            int seconds = Mathf.FloorToInt(totalTime % 60);

            strengthTimerText.text = "Strength: " + string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return null; 
        }

        VariableManager.Instance.weaponBonus = 0;
        strengthTimerText.text = "";
    }

    public void CraftHealPotion()
    {
        if (gelAmount > 0 && leavesAmount > 0 && bucketsAmount > 0)
        {
            gelAmount -= 1;
            leavesAmount -= 1;
            bucketsAmount -= 1;
            gelAmountText.text = gelAmount.ToString();
            gelAmountTextShop.text = gelAmount.ToString();
            leavesAmountText.text = leavesAmount.ToString();
            leavesAmountTextShop.text = leavesAmount.ToString();
            bucketsAmountText.text = bucketsAmount.ToString();
            bucketsAmountTextShop.text = bucketsAmount.ToString();
            healPotionAmount++;
            healPotionAmountText.text = healPotionAmount.ToString();
        }
    }

    public void CraftLargerHealPotion()
    {
        if (gelAmount > 2 && leavesAmount > 3 && bucketsAmount > 1)
        {
            gelAmount -= 3;
            leavesAmount -= 4;
            bucketsAmount -= 2;
            gelAmountText.text = gelAmount.ToString();
            gelAmountTextShop.text = gelAmount.ToString();
            leavesAmountText.text = leavesAmount.ToString();
            leavesAmountTextShop.text = leavesAmount.ToString();
            bucketsAmountText.text = bucketsAmount.ToString();
            bucketsAmountTextShop.text = bucketsAmount.ToString();
            largerHealPotionAmount++;
            largerHealPotionAmountText.text = largerHealPotionAmount.ToString();
        }
    }

    public void CraftSuperHealPotion()
    {
        if (gelAmount > 7 && leavesAmount > 10 && bucketsAmount > 2)
        {
            gelAmount -= 8;
            leavesAmount -= 11;
            bucketsAmount -= 3;
            gelAmountText.text = gelAmount.ToString();
            gelAmountTextShop.text = gelAmount.ToString();
            leavesAmountText.text = leavesAmount.ToString();
            leavesAmountTextShop.text = leavesAmount.ToString();
            bucketsAmountText.text = bucketsAmount.ToString();
            bucketsAmountTextShop.text = bucketsAmount.ToString();
            superHealPotionAmount++;
            superHealPotionAmountText.text = superHealPotionAmount.ToString();
        }
    }

    public void CraftStrengthPotion()
    {
        if (gelAmount > 7 && leavesAmount > 10 && bucketsAmount > 2)
        {
            gelAmount -= 8;
            leavesAmount -= 11;
            bucketsAmount -= 3;
            gelAmountText.text = gelAmount.ToString();
            gelAmountTextShop.text = gelAmount.ToString();
            leavesAmountText.text = leavesAmount.ToString();
            leavesAmountTextShop.text = leavesAmount.ToString();
            bucketsAmountText.text = bucketsAmount.ToString();
            bucketsAmountTextShop.text = bucketsAmount.ToString();
            strengthPotionAmount++;
            strengthPotionAmountText.text = strengthPotionAmount.ToString();
        }
    }

    public void BuyLeaf()
    {
        if (EconomyManager.Instance.SpendGold(1))
        {
            AddLeaf();
        }
    }

    public void BuyBucket()
    {
        if (EconomyManager.Instance.SpendGold(1))
        {
            AddBucket();
        }
    }

    public void BuyGel()
    {
        if (EconomyManager.Instance.SpendGold(1))
        {
            AddGel();
        }
    }

    public void AddLeaf()
    {
        leavesAmount++;
        leavesAmountText.text = leavesAmount.ToString();
        leavesAmountTextShop.text = leavesAmount.ToString();
    }

    public void AddBucket()
    {
        bucketsAmount++;
        bucketsAmountText.text = bucketsAmount.ToString();
        bucketsAmountTextShop.text = bucketsAmount.ToString();
    }

    public void AddGel()
    {
        gelAmount++;
        gelAmountText.text = gelAmount.ToString();
        gelAmountTextShop.text = gelAmount.ToString();
    }
}
