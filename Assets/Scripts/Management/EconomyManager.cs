using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    public int currentGold { get; private set; } = 0;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";

    public void UpdateCurrentGold(int amount = 1) {
        currentGold += amount;

        if (goldText == null) {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        goldText.text = currentGold.ToString("D3");
    }

    public bool SpendGold(int amount)
    {
        if (amount <= currentGold)
        {
            UpdateCurrentGold(-amount);
            return true;
        }

        return false;
    }
}
