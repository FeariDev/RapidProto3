using System;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    [SerializeField] PlayerStatistic[] playerStatistics;



    [Button]
    void InitializePlayerStatisticsArray()
    {
        PlayerStatistic.StatisticType[] array = (PlayerStatistic.StatisticType[])Enum.GetValues(typeof(PlayerStatistic.StatisticType));
        Array.Resize(ref playerStatistics, array.Length);

        for (int i = 0; i < playerStatistics.Length; i++)
        {
            if (playerStatistics[i] == null) playerStatistics[i] = new PlayerStatistic();
            else
            {
                playerStatistics[i].finalValue = playerStatistics[i].baseValue;
            }
            playerStatistics[i].statisticType = array[i];

            Debug.Log($"Initialized player statistic '{playerStatistics[i].statisticType}'");
        }

        Debug.Log($"Player statistics initialized!");
    }



    void OnInventoryUpdate(InventorySlot slot)
    {
        CardSO card = (CardSO)slot.itemType;

        ApplyCardModifier(card, slot.itemAmount);
    }

    void ApplyCardModifier(CardSO card, int cardAmount)
    {
        foreach (CardModifier modifier in card.cardModifiers)
        {
            for (int i = 0; i < playerStatistics.Length; i++)
            {
                bool isMatchingStatistic = playerStatistics[i].statisticType == modifier.modifierTarget;

                if (!isMatchingStatistic) continue;

                for (int j = 0; j <= playerStatistics[i].statisticModifiers.Length; j++)
                {
                    if (j == playerStatistics[i].statisticModifiers.Length)
                    {
                        Array.Resize(ref playerStatistics[i].statisticModifiers, playerStatistics[i].statisticModifiers.Length + 1);

                        StatisticModifier statisticModifier = new StatisticModifier();
                        statisticModifier.cardModifier = modifier;
                        statisticModifier.amount = cardAmount;
                        playerStatistics[i].statisticModifiers[playerStatistics[i].statisticModifiers.Length - 1] = statisticModifier;
                        break;
                    }

                    if (playerStatistics[i].statisticModifiers[j].cardModifier != modifier) continue;

                    playerStatistics[i].statisticModifiers[j].amount = cardAmount;
                    break;
                }

                playerStatistics[i].modifierFlat = 0;
                playerStatistics[i].modifierMultiplier = 0;

                foreach (StatisticModifier modifier1 in playerStatistics[i].statisticModifiers)
                {
                    switch (modifier1.cardModifier.modifierType)
                    {
                        case CardModifier.ModifierType.Flat:
                            playerStatistics[i].modifierFlat += modifier1.cardModifier.modifierValue * modifier1.amount;
                            break;
                        case CardModifier.ModifierType.Percentage:
                            playerStatistics[i].modifierMultiplier += modifier1.cardModifier.modifierValue / 100 * modifier1.amount;
                            break;
                    }
                }

                playerStatistics[i].finalValue = playerStatistics[i].baseValue + (playerStatistics[i].baseValue * playerStatistics[i].modifierMultiplier + playerStatistics[i].modifierFlat);
            }
        }
    }



    #region Unity lifecycle

    void Start()
    {
        InitializePlayerStatisticsArray();

        Player.Instance.inventory.OnInventoryUpdate += OnInventoryUpdate;
    }

    #endregion
}

[Serializable]
public class PlayerStatistic
{
    public StatisticType statisticType;
    public enum StatisticType
    {
        MoveSpeed,
        AttackSpeed,
        AttackDamage,
        Defense
    }

    public float baseValue;
    public float modifierFlat;
    public float modifierMultiplier;
    public float finalValue;

    public StatisticModifier[] statisticModifiers;
}

[Serializable]
public class StatisticModifier
{
    public CardModifier cardModifier;
    public int amount;
}
