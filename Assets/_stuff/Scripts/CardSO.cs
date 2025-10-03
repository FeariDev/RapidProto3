using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Items/Cards/New Card")]
public class CardSO : ItemSO
{
    [Header("Card Properties")]
    public CardModifier[] cardModifiers;
}

[Serializable]
public class CardModifier
{
    public float modifierValue;

    [Tooltip("This determines how the modifier adds the modifierAmount to a player statistic")]
    /// <summary>
    /// This determines how the modifier adds the modifierAmount to a player statistic
    /// </summary>
    public ModifierType modifierType;
    public enum ModifierType
    {
        Flat,
        Percentage
    }
    
    [Tooltip("This determines to what player statistic this modifier is added to")]
    /// <summary>
    /// This determines to what player statistic this modifier is added to
    /// </summary>
    public PlayerStatistic.StatisticType modifierTarget;
}