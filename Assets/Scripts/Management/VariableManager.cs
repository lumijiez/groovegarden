using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VariableManager : Singleton<VariableManager>
{
    [Range(1, 5)]
    public int weaponBonus = 0;

    [Range(1, 5)]
    public int speedBonus = 0;

    [Range(1, 5)]
    public int staminaRegenBonus = 1;

    public bool wasShrekKilled = false;
}
